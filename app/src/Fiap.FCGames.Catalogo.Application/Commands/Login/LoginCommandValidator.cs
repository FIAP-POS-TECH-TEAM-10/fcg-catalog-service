using FluentValidation;

namespace Fiap.FCGames.Catalogo.Application.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Usuario)
            .NotEmpty().WithMessage("O usuário é obrigatório.");

        RuleFor(c => c.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.");
    }
}
