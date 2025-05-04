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
    private const string _urlBase = "https://api.francetravail.io/partenaire/offresdemploi/v2/offres/search";

    public async Task<Result<string, Exception>> SearchOffresAsync(string codeInsee)
    {
        var token = await _tokenService.GetAccessTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) { return new Exception("waiting for token"); }

        _logger.Debug("token from pole emploi {t}", token);
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_urlBase}?commune={codeInsee}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.Error("Réponse erreur Pôle Emploi : {Content}", errorContent);
            return new Exception("Erreur lors de la recherche d’offres.");
        }

        return await response.Content.ReadAsStringAsync();
    }
}

