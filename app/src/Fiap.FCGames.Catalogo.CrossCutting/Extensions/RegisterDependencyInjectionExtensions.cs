using Fiap.FCGames.Catalogo.Domain.Interface.Service;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories;
using Fiap.FCGames.Catalogo.Infra.DataProvider.UnitOfWork;
using Fiap.FCGames.Catalogo.Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.FCGames.Catalogo.CrossCutting.Extensions;

public static class RegisterDependencyInjectionExtensions
{
    public static void RegisterDI(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IBibliotecaJogosRepository, BibliotecaJogosRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
    }
}
