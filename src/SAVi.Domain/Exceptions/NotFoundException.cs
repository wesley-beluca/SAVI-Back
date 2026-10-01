namespace SAVi.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"Entidade \"{entityName}\" ({key}) não foi encontrada.")
    {
    }
}
