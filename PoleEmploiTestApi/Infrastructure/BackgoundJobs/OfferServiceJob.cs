using Application.Dtos;
using Domain.Entities;
using Hangfire;
using Infrastructure.Abstraction;
using Infrastructure.Abstraction.Repositories;
using Serilog;
using System.Text.Json;

namespace Infrastructure.BackgoundJobs;

internal class OfferServiceJob(ILogger logger, IPoleEmploiApiClient apiClient, IJobOfferRepositoryReader repositoryReader,
    IJobOfferRepositoryWriter repositoryWriter) : IOfferServiceJob
{
    private readonly ILogger _logger = logger;
    private readonly IPoleEmploiApiClient _apiClient = apiClient;
    private readonly IJobOfferRepositoryReader _repositoryReader = repositoryReader;
    private readonly IJobOfferRepositoryWriter _repositoryWriter = repositoryWriter;
    //TODO RBO voir si à mettre en BDD dans une table Commune pour celles qui sont suivies
    private static readonly Dictionary<string, string> _cities = new()
    {
        ["VILLIERS LE DUC"] = "21704",
        //["Paris"] = "75056",
        //["Rennes"] = "",
        ["Bordeaux"] = "33063"
    };

    [DisableConcurrentExecution(timeoutInSeconds: 300)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new int[] { 60, 300, 600 }, LogEvents = true)]
    public async Task ImportOffersAsync(CancellationToken cancellationToken = default)
    {
        _logger.Information("Lancement de l'import");
        foreach (var city in _cities)
        {
            try
            {
                var json = await _apiClient.SearchOffresAsync(city.Value);
                if (!json.IsSuccess) { continue; }
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var searchResult = JsonSerializer.Deserialize<ApiSearchResponse>(json.Value, options);
                if (searchResult?.Resultats is not { Count: > 0 })
                {
                    continue;
                }

                var validOffers = searchResult.Resultats
                    .Where(o => !string.IsNullOrEmpty(o.Intitule) && !string.IsNullOrEmpty(o.OrigineOffre?.UrlOrigine))
                    .ToList();

                if (validOffers.Count == 0)
                {
                    continue;
                }
                var keys = validOffers.Select(o => (o.Intitule!, o.OrigineOffre!.UrlOrigine!)).ToList();
                var existingOffers = await _repositoryReader.GetExistingOffers(keys, cancellationToken);

                var now = DateTime.UtcNow;
                var newOffers = new List<JobOffer>();
                var offersToUpdate = new List<JobOffer>();

                foreach (var offre in validOffers)
                {
                    var key = (offre.Intitule!, offre.OrigineOffre!.UrlOrigine!);

                    if (existingOffers.TryGetValue(key, out var existingOffer))
                    {
                        offersToUpdate.Add(existingOffer);
                    }
                    else
                    {
                        newOffers.Add(new JobOffer
                        {
                            Id = Guid.CreateVersion7(),
                            Title = offre.Intitule!,
                            Description = offre.Description ?? string.Empty,
                            Url = offre.OrigineOffre.UrlOrigine!,
                            ContractType = offre.TypeContrat ?? "Inconnu",
                            Company = offre.Entreprise?.Nom ?? "Inconnue",
                            City = offre.LieuTravail?.Libelle ?? city.Key,
                            DatePosted = now,
                            LastUpdated = now
                        });
                    }
                }

                var insertCount = 0;
                var updateCount = 0;

                if (newOffers.Count != 0)
                {
                    await _repositoryWriter.BulkInsertAsync(newOffers, cancellationToken);
                    insertCount = newOffers.Count;
                }

                if (offersToUpdate.Count != 0)
                {
                    await _repositoryWriter.BulkUpdateAsync(offersToUpdate, cancellationToken);
                    updateCount = offersToUpdate.Count;
                }

                _logger.Information("Offres pour {Ville} traitées avec succès: {InsertCount} nouvelles offres, {UpdateCount} mises à jour",
                    city, insertCount, updateCount);
                _logger.Information("Offres pour {Ville} importées avec succès", city);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }
    }
}
