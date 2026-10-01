namespace SAVi.Application.Common.Interfaces;

// Nome "IdentityOperationResult" (não "IdentityResult") de propósito: evita colisão com
// Microsoft.AspNetCore.Identity.IdentityResult, usado pela implementação em Infrastructure.
public record IdentityOperationResult(bool Sucesso, string? Erro, Guid UsuarioId, string Email, string Nome);

public interface IIdentityService
{
    Task<IdentityOperationResult> RegistrarAsync(string email, string senha, string nome);

    Task<IdentityOperationResult> EntrarAsync(string email, string senha);

    Task<IdentityOperationResult> EntrarOuRegistrarComGoogleAsync(string googleSubject, string email, string nome);

    Task<IdentityOperationResult> AtualizarPerfilAsync(Guid usuarioId, string nome, string email);
}
