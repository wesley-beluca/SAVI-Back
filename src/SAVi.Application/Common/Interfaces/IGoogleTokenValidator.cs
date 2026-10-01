namespace SAVi.Application.Common.Interfaces;

public record GoogleUsuarioInfo(string Subject, string Email, string Nome);

public interface IGoogleTokenValidator
{
    Task<GoogleUsuarioInfo?> ValidarAsync(string idToken);
}
