namespace SAVi.Application.Common.Interfaces;

public record TokenGerado(string Token, DateTime ExpiraEmUtc);

public interface IJwtTokenService
{
    TokenGerado GerarToken(Guid usuarioId, string email, string nome);
}
