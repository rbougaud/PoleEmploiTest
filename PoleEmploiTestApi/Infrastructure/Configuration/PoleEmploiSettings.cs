namespace Infrastructure.Configuration;

public record PoleEmploiSettings
{
    public string ClientId { get; init; } = default!;
    public string ClientSecret { get; init; } = default!;
}
