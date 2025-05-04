using Hangfire;
using Infrastructure.Abstraction;

namespace Presentation.Extensions;

public static class BackgroudJobExtensions
{
    public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
    {
        string schedule = app.Configuration["BackgroundJobs:Schedule"]!;
        app.Services.GetRequiredService<IRecurringJobManager>().AddOrUpdate<IOfferServiceJob>("ImportOffers", job => job.ImportOffersAsync(CancellationToken.None), schedule);
        return app;
    }
}
