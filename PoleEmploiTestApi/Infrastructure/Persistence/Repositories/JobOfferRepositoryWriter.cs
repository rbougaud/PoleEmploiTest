using Domain.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Abstraction.Repositories;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories;

internal class JobOfferRepositoryWriter(WriterContext context) : IJobOfferRepositoryWriter
{
    private readonly WriterContext _context = context;

    public async Task BulkInsertAsync(List<JobOffer> newOffers, CancellationToken cancellationToken)
    {
        var bulkConfig = new BulkConfig
        {
            BatchSize = 1000,
            EnableStreaming = true,
            SetOutputIdentity = true,
            UseTempDB = true,
            PreserveInsertOrder = false
        };

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.BulkInsertAsync(newOffers, bulkConfig, cancellationToken: cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task BulkUpdateAsync(List<JobOffer> offersToUpdate, CancellationToken cancellationToken)
    {
        var bulkConfig = new BulkConfig
        {
            BatchSize = 1000,
            EnableStreaming = true,
            PropertiesToInclude =
            [
                nameof(JobOffer.Description),
                nameof(JobOffer.ContractType),
                nameof(JobOffer.Company),
                nameof(JobOffer.City),
                nameof(JobOffer.LastUpdated)
            ],
            PreserveInsertOrder = false
        };

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        await _context.BulkUpdateAsync(offersToUpdate, bulkConfig, cancellationToken: cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }


    public async Task AddAsync(JobOffer newOffre)
    {
        await _context.AddAsync(newOffre);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
