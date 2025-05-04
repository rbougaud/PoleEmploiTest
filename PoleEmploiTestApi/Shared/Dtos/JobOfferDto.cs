namespace Shared.Dtos;

public record JobOfferDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Company { get; init; }
    public required string ContractType { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public DateTime DatePosted { get; init; }
}
