namespace Infrastructure.Abstraction;

public interface IOfferServiceJob
{
    Task ImportOffersAsync(CancellationToken cancellationToken = default);
}
