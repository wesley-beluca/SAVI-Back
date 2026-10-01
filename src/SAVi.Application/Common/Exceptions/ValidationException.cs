using FluentValidation.Results;

namespace SAVi.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("Uma ou mais falhas de validação ocorreram.")
    {
        Erros = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Erros = failures
            .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }

    public IDictionary<string, string[]> Erros { get; }
}
