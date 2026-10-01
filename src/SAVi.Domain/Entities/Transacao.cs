using SAVi.Domain.Common;
using SAVi.Domain.Enums;

namespace SAVi.Domain.Entities;

public class Transacao : BaseEntity
{
    public required string Descricao { get; set; }

    public required decimal Valor { get; set; }

    public required DateOnly Data { get; set; }

    public required TipoTransacao Tipo { get; set; }

    public bool Recorrente { get; set; }

    public required string Conta { get; set; }

    public int Parcelas { get; set; } = 1;

    /// <summary>
    /// Quantidade de meses em que a transação se repete a partir de <see cref="Data"/> (1 = não repete).
    /// A recorrência é persistida como um único registro e projetada mês a mês nas consultas.
    /// </summary>
    public int MesesRecorrencia { get; set; } = 1;

    public required Guid CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }

    public required Guid UsuarioId { get; set; }

    /// <summary>
    /// Retorna o número da ocorrência (1-based) no mês informado, ou null se a transação não ocorre nele.
    /// </summary>
    public int? OcorrenciaNoMes(int ano, int mes)
    {
        var deslocamento = (ano - Data.Year) * 12 + (mes - Data.Month);
        return deslocamento >= 0 && deslocamento < MesesRecorrencia ? deslocamento + 1 : null;
    }

    public DateOnly DataDaOcorrencia(int ocorrencia) => Data.AddMonths(ocorrencia - 1);
}
