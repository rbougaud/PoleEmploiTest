using FluentValidation;
using Wolverine;
using Wolverine.FluentValidation;
using Application.Services.GenerateRapport;

namespace Presentation.Extensions;

internal static class ConfigurationWolverineExtension
{
    internal static void ConfigurationWolverine(this ConfigureHostBuilder host)
    {
        host.UseWolverine(options =>
        {
            options.Discovery.IncludeAssembly(typeof(Program).Assembly);
            options.UseFluentValidation();

            options.Discovery.IncludeAssembly(typeof(GenerateRapportOffersHandler).Assembly);
            //TODO RBO si besoin de rajouter des fluent validations
            //options.Services.AddValidatorsFromAssemblyContaining<GenerateRapportOffersCommand>();
        });
    }
}
