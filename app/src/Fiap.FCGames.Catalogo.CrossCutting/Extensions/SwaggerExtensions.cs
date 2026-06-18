using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Fiap.FCGames.Catalogo.CrossCutting.Extensions
{
    public static class SwaggerExtensions
    {
        public static void RegisterSwaggerGenerator(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Fiap - FCGAMES", Version = "v1" });                

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o Token desta forma Bearer"
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });

            });
        }


        public static void RegisterSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Fiap - FCGAMES");
            });
        }

        public static void RegisterScalar(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapScalarApiReference(options =>
            {
                options.Title = "API Fiap - FCGAMES";
                options.WithOpenApiRoutePattern("/openapi/{documentName}.json");
            });
        }
    }
}
