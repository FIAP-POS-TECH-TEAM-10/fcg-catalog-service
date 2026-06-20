using FluentValidation;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.AtualizarJogo;

public class AtualizarJogoCommandValidator : AbstractValidator<AtualizarJogoCommand>
{
    public AtualizarJogoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id é obrigatório.");
        RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome é obrigatório.").MaximumLength(200);
        RuleFor(x => x.Descricao).NotEmpty().WithMessage("Descrição é obrigatória.").MaximumLength(1000);
        RuleFor(x => x.Preco).GreaterThan(0).WithMessage("Preço deve ser maior que zero.");
    }
}
