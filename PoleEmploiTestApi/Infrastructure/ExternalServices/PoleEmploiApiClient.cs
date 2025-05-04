using Infrastructure.Abstraction;
using Serilog;
using Shared;
using StackExchange.Redis;
using System.Net.Http.Headers;

namespace Infrastructure.ExternalServices;

internal class PoleEmploiApiClient(ILogger logger, IConnectionMultiplexer redis, HttpClient httpClient, 
    IPoleEmploiTokenService tokenService) : IPoleEmploiApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger _logger = logger;
    private readonly IDatabase _database = redis.GetDatabase();
    private readonly IPoleEmploiTokenService _tokenService = tokenService;

    public async Task<Result<string, Exception>> SearchOffresAsync(string city)
    {
        var token = await _tokenService.GetAccessTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.pole-emploi.io/partenaire/offresdemploi/v2/offres/search?motsCles=&commune={city}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error("Erreur lors de la recherche d’offres : {StatusCode}", response.StatusCode);
            return new Exception("Erreur lors de la recherche d’offres.");
        }

        return await response.Content.ReadAsStringAsync();
    }

}


