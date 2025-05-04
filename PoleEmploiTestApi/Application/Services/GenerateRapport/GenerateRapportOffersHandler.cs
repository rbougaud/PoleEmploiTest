using Infrastructure.Abstraction.Repositories;
using Serilog;

namespace Application.Services.GenerateRapport;

public class GenerateRapportOffersHandler(ILogger logger, IJobOfferRepositoryReader repositoryReader)
{
    private readonly ILogger _logger = logger;
    private readonly IJobOfferRepositoryReader _repositoryReader = repositoryReader;

    public async Task Handle(GenerateRapportOffersCommand command, CancellationToken cancellationToken)
    {
        var stats = await _repositoryReader.GetOfferStats(cancellationToken);
        _logger.Information("{c} lines number on .txt {count}", nameof(GenerateRapportOffersHandler), stats.Count);
        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

        var fileName = $"rapport_offres_{DateTime.UtcNow:yyyyMMddHHmmss}.txt";
        var fullPath = Path.Combine(downloadsPath, fileName);

        using var writer = new StreamWriter(fullPath);

        writer.WriteLine("Statistiques des Offres Pôle Emploi");
        writer.WriteLine("=====================================");
        foreach (var stat in stats)
        {
            var line = $"Contrat: {stat.ContractType,-20} | Entreprise: {stat.Company,-30} | Pays: {stat.Country,-15} | Nombre: {stat.Count}";
            writer.WriteLine(line);
        }

        _logger.Information("Rapport généré avec succès : {FileName}", fileName);
    }
}
