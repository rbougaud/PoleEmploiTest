using Domain.Entities;

namespace Infrastructure.Abstraction.Repositories;

public interface IJobOfferRepositoryWriter
{
    Task AddAsync(JobOffer newOffre);
    Task BulkInsertAsync(List<JobOffer> newOffers, CancellationToken cancellationToken);
    Task BulkUpdateAsync(List<JobOffer> offersToUpdate, CancellationToken cancellationToken);
    Task SaveAsync();
}
