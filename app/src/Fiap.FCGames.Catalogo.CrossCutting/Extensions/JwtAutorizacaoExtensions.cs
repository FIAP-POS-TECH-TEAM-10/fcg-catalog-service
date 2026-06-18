using Microsoft.Extensions.DependencyInjection;

namespace Fiap.FCGames.Catalogo.CrossCutting.Extensions
{
    public static class JwtAutorizacaoExtensions
    {
        public static void AddAutorizacaoApi(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });
        }
    }
}