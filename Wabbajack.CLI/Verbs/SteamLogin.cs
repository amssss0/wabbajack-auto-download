using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wabbajack.CLI.Builder;
using Wabbajack.Networking.Http.Interfaces;
using Wabbajack.Networking.Steam;
using Wabbajack.Paths;

namespace Wabbajack.CLI.Verbs;

public class SteamLogin
{
    private readonly ILogger<SteamLogin> _logger;
    private readonly Client _client;
    private readonly ITokenProvider<SteamLoginState> _token;

    public SteamLogin(ILogger<SteamLogin> logger, Client steamClient, ITokenProvider<SteamLoginState> token)
    {
        _logger = logger;
        _client = steamClient;
        _token = token;
    }

    public static VerbDefinition Definition = new("steam-login",
        "Logs into Steam via interactive prompts", new[]
        {
            new OptionDefinition(typeof(string), "u", "user", "Username for login")
        });

    public async Task<int> Run(string user)
    {
        var token = await _token.Get();
        
        if (token == null || token.User != user || string.IsNullOrWhiteSpace(token.Password))
        {
            Console.WriteLine("Please enter password");
            var password = Console.ReadLine() ?? "";
            
            await _token.SetToken(new SteamLoginState
            {
                User = user,
                Password = password.Trim()
            });
        }
        
        _logger.LogInformation("Attempting login");
        await _client.Login();

        await Task.Delay(10000);

        return 0;
    }
    
}