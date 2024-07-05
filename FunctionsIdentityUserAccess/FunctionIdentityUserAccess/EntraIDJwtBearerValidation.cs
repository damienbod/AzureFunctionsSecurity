using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FunctionIdentityUserAccess;

public class EntraIDJwtBearerValidation
{
    private IConfiguration _configuration;
    private ILogger _log;
    private const string scopeType = @"http://schemas.microsoft.com/identity/claims/scope";
    private ConfigurationManager<OpenIdConnectConfiguration>? _configurationManager;

    private string _wellKnownEndpoint = string.Empty;
    private string? _tenantId = string.Empty;
    private string? _audience = string.Empty;
    private string? _instance = string.Empty;
    private string _requiredScope = "access_as_user";

    public EntraIDJwtBearerValidation(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _log = loggerFactory.CreateLogger<EntraIDJwtBearerValidation>();

        _tenantId = _configuration["AzureAd:TenantId"];
        _audience = _configuration["AzureAd:ClientId"];
        _instance = _configuration["AzureAd:Instance"];

        if (_tenantId == null || _audience == null || _instance == null)
        {
            throw new ArgumentException("missing API configuration");
        }

        _wellKnownEndpoint = $"{_instance}{_tenantId}/v2.0/.well-known/openid-configuration";
    }

    public async Task<TokenValidationResult?> ValidateTokenAsync(string? authorizationHeader)
    {
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return null;
        }

        if (!authorizationHeader.Contains("Bearer"))
        {
            return null;
        }

        var accessToken = authorizationHeader.Substring("Bearer ".Length);

        var oidcWellknownEndpoints = await GetOIDCWellknownConfiguration();

        var tokenValidator = new JsonWebTokenHandler
        {
            MapInboundClaims = false
        };

        var validationParameters = new TokenValidationParameters
        {
            RequireSignedTokens = true,
            ValidAudience = _audience,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKeys = oidcWellknownEndpoints.SigningKeys,
            ValidIssuer = oidcWellknownEndpoints.Issuer
        };

        try
        {
            var tokenValidationResult = await tokenValidator.ValidateTokenAsync(accessToken, validationParameters);

            if (tokenValidationResult.IsValid && IsScopeValid(_requiredScope, tokenValidationResult.ClaimsIdentity))
            {
                return tokenValidationResult;
            }

            return null;
        }
        catch (Exception ex)
        {
            _log.LogError(ex.ToString());
        }
        return null;
    }

    public string GetPreferredUserName(ClaimsIdentity claimsIdentity)
    {
        var preferredUsername = string.Empty;
        if (claimsIdentity != null)
        {
            var preferred_username = claimsIdentity.Claims.FirstOrDefault(t => t.Type == "preferred_username");
            if (preferred_username != null)
            {
                preferredUsername = preferred_username.Value;
            }
        }

        return preferredUsername;
    }

    private async Task<OpenIdConnectConfiguration> GetOIDCWellknownConfiguration()
    {
        _log.LogDebug("Get OIDC well known endpoints {_wellKnownEndpoint}", _wellKnownEndpoint);
        _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
             _wellKnownEndpoint, new OpenIdConnectConfigurationRetriever());

        return await _configurationManager.GetConfigurationAsync();
    }

    private bool IsScopeValid(string scopeName, ClaimsIdentity claimsIdentity)
    {
        if (claimsIdentity == null)
        {
            _log.LogWarning("Scope invalid {scopeName}", scopeName);
            return false;
        }

        var scopeClaim = claimsIdentity.HasClaim(x => x.Type == "scp")
            ? claimsIdentity.Claims.First(x => x.Type == "scp").Value
            : string.Empty;

        // fallback for MS mapping
        if (string.IsNullOrEmpty(scopeClaim))
        {
            scopeClaim = claimsIdentity.HasClaim(x => x.Type == scopeType)
            ? claimsIdentity.Claims.First(x => x.Type == scopeType).Value
            : string.Empty;
        }

        if (string.IsNullOrEmpty(scopeClaim))
        {
            _log.LogWarning("Scope invalid {scopeName}", scopeName);
            return false;
        }

        if (!scopeClaim.Equals(scopeName, StringComparison.OrdinalIgnoreCase))
        {
            _log.LogWarning("Scope invalid {scopeName}", scopeName);
            return false;
        }

        _log.LogDebug("Scope valid {scopeName}", scopeName);
        return true;
    }
}
