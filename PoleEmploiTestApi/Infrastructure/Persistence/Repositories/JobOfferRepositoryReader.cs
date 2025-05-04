using Domain.Entities;
using Infrastructure.Abstraction.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace Infrastructure.Persistence.Repositories;

internal class JobOfferRepositoryReader(ReaderContext context) : IJobOfferRepositoryReader
{
    private readonly ReaderContext _context = context;

    public async Task<bool> CheckIfExist(string? titre, string? url, CancellationToken cancellationToken)
    {
        return await _context.JobOffers.AnyAsync(o => o.Title == titre && o.Url == url, cancellationToken);
    }

    public async Task<Dictionary<(string Title, string Url), JobOffer>> GetExistingOffers(
        List<(string Title, string Url)> keysToLookup, CancellationToken cancellationToken)
    {
        var titles = keysToLookup.Select(k => k.Title).Distinct().ToList();
        var urls = keysToLookup.Select(k => k.Url).Distinct().ToList();

        var query = await _context.JobOffers
            .Where(o => titles.Contains(o.Title) && urls.Contains(o.Url))
            .ToListAsync(cancellationToken);

        return query
            .DistinctBy(o => (o.Title, o.Url))
            .ToDictionary(o => (o.Title, o.Url), o => o);
    }

    public async Task<List<OfferStatDto>> GetOfferStats(CancellationToken cancellationToken)
    {
        return await _context.JobOffers
            .GroupBy(o => new { o.ContractType, o.Company, o.Country })
            .Select(g => new OfferStatDto
            {
                ContractType = g.Key.ContractType,
                Company = g.Key.Company,
                Country = g.Key.Country,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);
    }
}
