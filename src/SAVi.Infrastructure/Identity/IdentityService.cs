using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;

namespace SAVi.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<IdentityOperationResult> RegistrarAsync(string email, string senha, string nome)
    {
        var existente = await userManager.FindByEmailAsync(email);
        if (existente is not null)
        {
            return new IdentityOperationResult(false, "Já existe uma conta com esse email.", Guid.Empty, email, nome);
        }

        var usuario = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            Nome = nome
        };

        var resultado = await userManager.CreateAsync(usuario, senha);

        if (!resultado.Succeeded)
        {
            var erro = string.Join("; ", resultado.Errors.Select(e => e.Description));
            return new IdentityOperationResult(false, erro, Guid.Empty, email, nome);
        }

        return new IdentityOperationResult(true, null, usuario.Id, usuario.Email!, usuario.Nome);
    }

    public async Task<IdentityOperationResult> EntrarAsync(string email, string senha)
    {
        var usuario = await userManager.FindByEmailAsync(email);

        if (usuario is null || !await userManager.CheckPasswordAsync(usuario, senha))
        {
            return new IdentityOperationResult(false, "Email ou senha inválidos.", Guid.Empty, email, string.Empty);
        }

        return new IdentityOperationResult(true, null, usuario.Id, usuario.Email!, usuario.Nome);
    }

    public async Task<IdentityOperationResult> EntrarOuRegistrarComGoogleAsync(string googleSubject, string email, string nome)
    {
        var usuario = await userManager.Users
            .SingleOrDefaultAsync(u => u.GoogleSubject == googleSubject || u.Email == email);

        if (usuario is not null)
        {
            if (usuario.GoogleSubject is null)
            {
                usuario.GoogleSubject = googleSubject;
                await userManager.UpdateAsync(usuario);
            }

            return new IdentityOperationResult(true, null, usuario.Id, usuario.Email!, usuario.Nome);
        }

        var novoUsuario = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            Nome = nome,
            GoogleSubject = googleSubject,
            EmailConfirmed = true
        };

        var resultado = await userManager.CreateAsync(novoUsuario);

        if (!resultado.Succeeded)
        {
            var erro = string.Join("; ", resultado.Errors.Select(e => e.Description));
            return new IdentityOperationResult(false, erro, Guid.Empty, email, nome);
        }

        return new IdentityOperationResult(true, null, novoUsuario.Id, novoUsuario.Email!, novoUsuario.Nome);
    }

    public async Task<IdentityOperationResult> AtualizarPerfilAsync(Guid usuarioId, string nome, string email)
    {
        var usuario = await userManager.FindByIdAsync(usuarioId.ToString());
        if (usuario is null)
        {
            return new IdentityOperationResult(false, "Usuário não encontrado.", usuarioId, email, nome);
        }

        var existente = await userManager.FindByEmailAsync(email);
        if (existente is not null && existente.Id != usuarioId)
        {
            return new IdentityOperationResult(false, "Já existe uma conta com esse email.", usuarioId, email, nome);
        }

        usuario.Nome = nome;
        usuario.Email = email;
        usuario.UserName = email;

        var resultado = await userManager.UpdateAsync(usuario);

        if (!resultado.Succeeded)
        {
            var erro = string.Join("; ", resultado.Errors.Select(e => e.Description));
            return new IdentityOperationResult(false, erro, usuarioId, email, nome);
        }

        return new IdentityOperationResult(true, null, usuario.Id, usuario.Email!, usuario.Nome);
    }
}
