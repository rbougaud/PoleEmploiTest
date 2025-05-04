using Shared;

namespace Infrastructure.Abstraction;

public interface IPoleEmploiApiClient
{
    Task<Result<string, Exception>> SearchOffresAsync(string city);
}

