using Hangfire;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Presentation.Filters;
using Presentation.Transformers;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

try
{
    Log.Logger.Debug("Starting up");
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddUserSecrets<Program>();
    builder.Logging.ClearProviders();
    //TODO RBO OpenTelemetry + Prometheus si tu veux aller très loin
    builder.Host.UseSerilog((context, cfg) =>
    {
        cfg.Enrich.FromLogContext()
            .Filter.With<ApplicationLogWithRequestsFilter>()
            .WriteTo.Console()
            .MinimumLevel.Debug();
    });

    builder.Services
    .AddAuthorization()
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("hkfgsdkjfgsdkj")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    });

    builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection")!, builder.Configuration.GetConnectionString("Redis"));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options => //scalar/v1
        {
            options
            .WithTitle("SpotifyTestApi")
            .WithTheme(ScalarTheme.Moon)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            AppPath = "/", // retour à l'API root
            Authorization = [],
            DashboardTitle = "Spotify Refresh Jobs",
            DisplayStorageConnectionString = false,
            IsReadOnlyFunc = context => false, // Dashboard interactif
            StatsPollingInterval = 2000, // refresh toutes les 2 secondes
        });
    }

    app.UseRouting();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    //HealthChecks
    app.MapHealthChecks("/healthz");
    app.MapHealthChecksUI();
    Log.Logger.Debug("App is running");
    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex.Message);
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
