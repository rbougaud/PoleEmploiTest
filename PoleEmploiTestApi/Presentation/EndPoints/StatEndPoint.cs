using Application.Services.GenerateRapport;
using Wolverine;

namespace Presentation.EndPoints;

public static class StatEndPoint
{
    public static void MapStatEndPoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/rapport/offers", async (IMessageBus bus) =>
        {
            await bus.InvokeAsync(new GenerateRapportOffersCommand());
            return Results.Ok("Rapport généré avec succès.");
        });
    }
}
