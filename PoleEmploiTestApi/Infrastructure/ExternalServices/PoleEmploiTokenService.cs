using Infrastructure.Abstraction;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.ExternalServices;

internal class PoleEmploiTokenService(ILogger logger, IOptions<PoleEmploiSettings> settings, IConnectionMultiplexer redis, HttpClient httpClient) : IPoleEmploiTokenService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(2));
    private readonly IOptions<PoleEmploiSettings> _settings = settings;
    private readonly ILogger _logger = logger;
    private readonly IDatabase _db = redis.GetDatabase();
    private const string TokenCacheKey = "poleemploi:access_token";

    public async Task<string> GetAccessTokenAsync()
    {
        var token = await _db.StringGetAsync(TokenCacheKey);
        if (!token.IsNullOrEmpty)
        {
            return token!;
        }

        var clientId = _settings.Value.ClientId;
        var clientSecret = _settings.Value.ClientSecret;

        var content = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("scope", "api_offresdemploiv2 o2dsoffre")
        ]);

        try
        {
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsync("https://entreprise.pole-emploi.fr/connexion/oauth2/access_token?realm=/partenaire", content)
            );

            if (!response.IsSuccessStatusCode)
            {
                _logger.Error("Erreur lors de l'obtention du token Pôle Emploi : {Code}", response.StatusCode);
                throw new ApplicationException("Échec d'authentification avec Pôle Emploi.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PoleEmploiTokenResponse>(json)!;

            await _db.StringSetAsync(TokenCacheKey, result.AccessToken, TimeSpan.FromSeconds(result.ExpiresIn - 60));
            return result.AccessToken;
        }
        catch(Exception ex)
        {
            _logger.Error(ex, ex.Message);
            throw;
        }
    }

    private class PoleEmploiTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = null!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
