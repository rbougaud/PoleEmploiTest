using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.Redis.StackExchange;
using Infrastructure.Abstraction;
using Infrastructure.Abstraction.Repositories;
using Infrastructure.BackgoundJobs;
using Infrastructure.Configuration;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, string? redisConnectionString, IConfigurationSection configurationSection)
    {
        services.TryAddSingleton<Serilog.ILogger>();
        services.Configure<PoleEmploiSettings>(configurationSection);
        services.AddDbContext<WriterContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(WriterContext).Assembly.FullName);
            });
        });
        services.AddDbContext<ReaderContext>(options => options.UseNpgsql(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IJobOfferRepositoryWriter, JobOfferRepositoryWriter>();
        services.AddScoped<IJobOfferRepositoryReader, JobOfferRepositoryReader>();

        // Redis
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var redisCs = redisConnectionString ?? "localhost:6379";
            return ConnectionMultiplexer.Connect(redisCs);
        });

        // Hangfire
        var redisConnectionStringForHangfire = redisConnectionString ?? "localhost:6379";
        services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(options =>
            {
                options.UseNpgsqlConnection(connectionString);
            });
            config.UseRedisStorage(redisConnectionStringForHangfire, new RedisStorageOptions
            {
                Prefix = "hangfire:poleemploi:",
                InvisibilityTimeout = TimeSpan.FromMinutes(1)
            });
        });
        services.AddHangfireServer();
        services.AddScoped<IOfferServiceJob, OfferServiceJob>();

        services.AddSingleton<IPoleEmploiTokenService, PoleEmploiTokenService>();
        services.AddHttpClient<IPoleEmploiTokenService, PoleEmploiTokenService>();
        services.AddHttpClient<IPoleEmploiApiClient, PoleEmploiApiClient>();
        services.AddHealthChecks().AddNpgSql(connectionString).AddRedis(redisConnectionStringForHangfire!);
        services.AddHealthChecksUI(setupSettings: settings =>
        {
            settings.AddHealthCheckEndpoint("default", "/healthz");
        }).AddInMemoryStorage();

        return services;
    }
}
