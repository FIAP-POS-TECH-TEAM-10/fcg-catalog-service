using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Domain.Aggregates;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.BibliotecaJogos.BuscarBibliotecaJogos;

public class BuscarBibliotecaJogosQueryHandler : IRequestHandler<BuscarBibliotecaJogosQuery, BuscarBibliotecaJogosResponse?>
{
    private readonly IBibliotecaJogosRepository _bibliotecaRepo;

    public BuscarBibliotecaJogosQueryHandler(IBibliotecaJogosRepository bibliotecaRepo)
    {
        _bibliotecaRepo = bibliotecaRepo;
    }

    public async Task<BuscarBibliotecaJogosResponse?> Handle(BuscarBibliotecaJogosQuery request, CancellationToken cancellationToken)
    {
        var biblioteca = await _bibliotecaRepo.ObterPorUsuarioIdAsync(new UsuarioId(request.UsuarioId));

        if (biblioteca is null)
            throw new NotFoundException("Biblioteca não encontrada para este usuário.");

        return new BuscarBibliotecaJogosResponse
        {
            DataCriacao = biblioteca.DataCriacao
        };
    }
}
