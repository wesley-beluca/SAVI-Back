namespace SAVi.Application.Auth.Common;

public record UsuarioDto(Guid Id, string Email, string Nome);

public record AuthResponseDto(string Token, DateTime ExpiraEmUtc, UsuarioDto Usuario);
