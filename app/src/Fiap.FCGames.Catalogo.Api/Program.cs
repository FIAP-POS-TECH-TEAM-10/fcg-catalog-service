using Fiap.FCGames.Catalogo.CrossCutting.Extensions;
using Fiap.FCGames.Catalogo.CrossCutting.Middleware;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Contexto;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Seed;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.RegisterDI();
builder.Services.AddMediatRConfiguration();

builder.Services.RegisterSwaggerGenerator();

builder.Services.AddAutenticacaoApi(builder.Configuration);

builder.Services.AddAutorizacaoApi();

builder.Services.AddContextDatabase(builder.Configuration);

builder.Services.AddMassTransitRabbitMq(builder.Configuration);

builder.Services.AddHealthChecks()
    // Check "self": sempre saudável, não depende de RabbitMQ/MassTransit — usado pelo
    // healthcheck de container (Docker/ECS), que precisa responder mesmo com o broker fora do ar.
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Aplica automaticamente as migrations pendentes e cria o banco SQLite local na inicialização.
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<FcGamesContexto>();
    dbContext.Database.Migrate();
    await SeedData.SeedJogosAsync(dbContext);
}

app.UseCorrelationId();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.RegisterSwagger();
    app.MapOpenApi();
    app.RegisterScalar();
}
app.UseErrorHandlingMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// /health: agregado completo (inclui o check do MassTransit/RabbitMQ) — usado pelo
// docker-compose/k8s, que já garantem o RabbitMQ saudável antes de checar este serviço.
// ResponseWriter detalha em JSON o nome/status de cada check (prova de qual check falha).
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var payload = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                tags = e.Value.Tags,
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(payload);
    }
});
// /health/live: só confirma que o processo subiu, sem depender do broker — usado pelo
// healthcheck de container (ECS), que não tem RabbitMQ nesta infra.
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

await app.RunAsync();
