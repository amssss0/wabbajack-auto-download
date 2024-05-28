using Microsoft.Extensions.Logging;
using Wabbajack.DTOs.JsonConverters;
using Wabbajack.DTOs.Logins;
using Wabbajack.Networking.NexusApi;

namespace Wabbajack.Services.OSIntegrated.TokenProviders;

public class NexusApiTokenProvider : EncryptedJsonTokenProvider<NexusApiState>, ApiKey
{
    public NexusApiTokenProvider(ILogger<NexusApiTokenProvider> logger, DTOSerializer dtos) : base(logger, dtos,
        "nexus-login")
    {
    }
}