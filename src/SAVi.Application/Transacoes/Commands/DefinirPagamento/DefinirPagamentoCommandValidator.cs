using FluentValidation;

namespace SAVi.Application.Transacoes.Commands.DefinirPagamento;

public class DefinirPagamentoCommandValidator : AbstractValidator<DefinirPagamentoCommand>
{
    public DefinirPagamentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Ocorrencia).GreaterThanOrEqualTo(1);
    }
}
