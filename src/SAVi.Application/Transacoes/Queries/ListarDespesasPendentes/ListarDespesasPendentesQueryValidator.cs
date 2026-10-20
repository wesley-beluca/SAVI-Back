using FluentValidation;

namespace SAVi.Application.Transacoes.Queries.ListarDespesasPendentes;

public class ListarDespesasPendentesQueryValidator : AbstractValidator<ListarDespesasPendentesQuery>
{
    private const int MaximoDiasIntervalo = 366;

    public ListarDespesasPendentesQueryValidator()
    {
        RuleFor(x => x.Ate)
            .GreaterThanOrEqualTo(x => x.De)
            .Must((query, ate) => ate.DayNumber - query.De.DayNumber <= MaximoDiasIntervalo)
            .WithMessage($"O intervalo deve ter no máximo {MaximoDiasIntervalo} dias.");
    }
}
