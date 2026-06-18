using Fiap.FCGames.Catalogo.Domain.Aggregates;
using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Domain.Interface.Service;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Usuario.AtualizarUsuario;

public class AtualizarUsuarioCommandHandler : IRequestHandler<AtualizarUsuarioCommand, AtualizarUsuarioResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasherService _passwordHasherService;

    public AtualizarUsuarioCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasherService passwordHasherService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<AtualizarUsuarioResponse> Handle(AtualizarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _unitOfWork.UsuarioRepository.ObterPorIdAsync(new UsuarioId(request.Id));

        if (usuario is null)
            throw new NotFoundException("Usuário não encontrado.");

        usuario.Nome = request.Nome;
        usuario.Email = request.Email;
        usuario.Senha = _passwordHasherService.GerarHash(request.Senha);
        usuario.NomeUsuario = request.NomeUsuario.ToLower();

        _unitOfWork.UsuarioRepository.Atualizar(usuario);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new AtualizarUsuarioResponse
        {
            Id = usuario.Id.Value,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }
}
