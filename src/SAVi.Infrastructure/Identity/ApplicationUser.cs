using Microsoft.AspNetCore.Identity;

namespace SAVi.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public required string Nome { get; set; }

    public string? GoogleSubject { get; set; }
}
