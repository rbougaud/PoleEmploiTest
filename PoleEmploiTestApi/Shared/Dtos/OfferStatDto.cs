namespace Application.Dtos;

public record OfferStatDto
{
    public required string ContractType { get; init; }
    public required string Company { get; init; }
    public string? Country { get; init; } 
    public int Count { get; init; }
}
