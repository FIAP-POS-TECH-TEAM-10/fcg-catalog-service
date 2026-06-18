using Fiap.FCGames.Catalogo.Domain.Aggregates;
using Fiap.FCGames.Catalogo.Domain.Interface.Service;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Contexto;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    private readonly IPasswordHasherService _passwordHasherService;

    public UsuarioRepository(FcGamesContexto context, IPasswordHasherService passwordHasherService) : base(context)
    {
        _passwordHasherService = passwordHasherService;
    }

    public void Adicionar(Usuario usuario) => Create(usuario);

    public void Atualizar(Usuario usuario) => Update(usuario);

    public async Task<IEnumerable<Usuario>> ObterTodosAsync()
    {
        return await _dbSet.AsNoTracking()
            .Include(x => x.TipoAcessoUsuario)
            .ToListAsync();
    }

    public async Task<Usuario?> ObterAsync(string usuario, string senha)
    {
        var usuarioDb = await _dbSet.AsNoTracking()
            .Include(x => x.TipoAcessoUsuario)
            .Where(x => x.NomeUsuario.ToLower().Equals(usuario.ToLower()))
            .FirstOrDefaultAsync();

        if (usuarioDb is null)
            return null;

        return _passwordHasherService.Verificar(senha, usuarioDb.Senha)
            ? usuarioDb
            : null;
    }

    public async Task<Usuario?> ObterPorIdAsync(UsuarioId id)
    {
        return await _dbSet.AsNoTracking()
            .Include(x => x.TipoAcessoUsuario)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public Task<bool> ExisteEmailAsync(string email)
        => _dbSet.AsNoTracking().AnyAsync(x => x.Email.ToLower() == email.ToLower());

    public Task<bool> ExisteNomeUsuarioAsync(string nomeUsuario)
        => _dbSet.AsNoTracking().AnyAsync(x => x.NomeUsuario.ToLower() == nomeUsuario.ToLower());
}
