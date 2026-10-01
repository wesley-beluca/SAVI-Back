namespace SAVi.Application.Orcamentos.Common;

public record OrcamentoDto(int Mes, int Ano, decimal RendaPrevista, decimal LimitePlanejado, decimal Utilizado)
{
    public decimal Disponivel => LimitePlanejado - Utilizado;
}
