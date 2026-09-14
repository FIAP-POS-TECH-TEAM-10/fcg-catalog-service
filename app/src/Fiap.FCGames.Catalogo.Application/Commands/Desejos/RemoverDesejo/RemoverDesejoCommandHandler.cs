using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Desejos.RemoverDesejo;

public class RemoverDesejoCommandHandler : IRequestHandler<RemoverDesejoCommand, Unit>
{
    private readonly IDesejoRepository _desejoRepository;

    public RemoverDesejoCommandHandler(IDesejoRepository desejoRepository) => _desejoRepository = desejoRepository;

    public async Task<Unit> Handle(RemoverDesejoCommand request, CancellationToken cancellationToken)
    {
        var desejo = await _desejoRepository.ObterAsync(request.UsuarioId, request.JogoId)
            ?? throw new NotFoundException($"Jogo '{request.JogoId}' não está na lista de desejos.");

        await _desejoRepository.RemoverAsync(desejo.UsuarioId, desejo.JogoId);

        return Unit.Value;
    }
}
