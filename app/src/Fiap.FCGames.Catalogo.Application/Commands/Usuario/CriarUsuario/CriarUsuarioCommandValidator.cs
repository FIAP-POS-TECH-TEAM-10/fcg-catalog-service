using FluentValidation;

namespace Fiap.FCGames.Catalogo.Application.Commands.Usuario.CriarUsuario;

public class CriarUsuarioCommandValidator : AbstractValidator<CriarUsuarioCommand>
{
    public CriarUsuarioCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("O email deve ser válido.")
            .EmailAddress().WithMessage("O email deve ser válido.");

        RuleFor(c => c.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]+$")
                   .WithMessage("A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere destes @$!%*?&#.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.");
    }
}
