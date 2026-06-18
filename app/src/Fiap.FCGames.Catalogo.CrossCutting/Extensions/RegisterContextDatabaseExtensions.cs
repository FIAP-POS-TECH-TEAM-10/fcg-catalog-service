using Fiap.FCGames.Catalogo.Infra.DataProvider.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.FCGames.Catalogo.CrossCutting.Extensions
{
    public static class RegisterContextDatabaseExtensions
    {
        public static void AddContextDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FcGamesContexto>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
            );
        }
    }
}
