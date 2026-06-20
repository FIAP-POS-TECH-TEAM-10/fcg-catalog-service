using FluentValidation;

namespace Fiap.FCGames.Catalogo.Application.Commands.Compras.RealizarCompra;

public class RealizarCompraCommandValidator : AbstractValidator<RealizarCompraCommand>
{
    public RealizarCompraCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("UsuarioId é obrigatório.");
        RuleFor(x => x.JogoId).NotEmpty().WithMessage("JogoId é obrigatório.");
    }
}
