using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SAVi.Application.Common.Interfaces;

namespace SAVi.Infrastructure.Identity;

public class GoogleTokenValidator(IOptions<GoogleOptions> options, ILogger<GoogleTokenValidator> logger)
    : IGoogleTokenValidator
{
    private readonly GoogleOptions _options = options.Value;

    public async Task<GoogleUsuarioInfo?> ValidarAsync(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_options.ClientId]
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            return new GoogleUsuarioInfo(payload.Subject, payload.Email, payload.Name ?? payload.Email);
        }
        catch (InvalidJwtException ex)
        {
            logger.LogWarning(ex, "ID token do Google inválido.");
            return null;
        }
    }
}
