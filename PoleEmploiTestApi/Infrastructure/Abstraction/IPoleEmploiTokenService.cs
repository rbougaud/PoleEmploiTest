namespace Infrastructure.Abstraction;

public interface IPoleEmploiTokenService
{
    Task<string> GetAccessTokenAsync();
}

