using Domain.Entities;
using Shared.Dtos;

namespace Infrastructure.Abstraction.Repositories;

public interface IJobOfferRepositoryReader
{
    Task<bool> CheckIfExist(string? titre, string? url, CancellationToken cancellationToken);
    Task<Dictionary<(string Title, string Url), JobOffer>> GetExistingOffers(
        List<(string Title, string Url)> keysToLookup, CancellationToken cancellationToken);
    Task<List<OfferStatDto>> GetOfferStats(CancellationToken cancellationToken);
}
