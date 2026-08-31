using FluentValidation;

namespace Fiap.FCGames.Catalogo.Application.Commands.Desejos.AdicionarDesejo;

public class AdicionarDesejoCommandValidator : AbstractValidator<AdicionarDesejoCommand>
{
    public AdicionarDesejoCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("UsuarioId é obrigatório.");
        RuleFor(x => x.JogoId).NotEmpty().WithMessage("JogoId é obrigatório.");
    }
}
