using Serilog;

namespace Application.Services.GenerateRapport;

public class GenerateRapportOffersHandler(ILogger logger)//, IJobOfferRepositoryReader)//, IJobOfferRepositoryReader repositoryReader)
{
    private readonly ILogger _logger = logger;

    public async Task Handle(GenerateRapportOffersCommand command, CancellationToken cancellationToken)
    {
        //await _reportingService.GenerateRapportAsync(cancellationToken);

        //var fileName = $"rapport_offres_{DateTime.UtcNow:yyyyMMddHHmmss}.txt";
        //using var writer = new StreamWriter(fileName);

        //writer.WriteLine("Statistiques des Offres Pôle Emploi");
        //writer.WriteLine("=====================================");
        //foreach (var stat in stats)
        //{
        //    var line = $"Contrat: {stat.TypeContrat,-20} | Entreprise: {stat.Entreprise,-30} | Pays: {stat.Pays,-15} | Nombre: {stat.Nombre}";
        //    _logger.Information(line);
        //    writer.WriteLine(line);
        //}

        //_logger.Information("Rapport généré avec succès : {FileName}", fileName);
    }
}
