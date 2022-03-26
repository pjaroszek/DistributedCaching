namespace DistributedCachingPoC.Infrastructure;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.SqlServer.Dac;
using Microsoft.Data.SqlClient;
using MediatR;
using Microsoft.Extensions.Logging;
using DistributedCachingPoC.Infrastructure.Interfaces;
using DistributedCachingPoC.Infrastructure.Services;

public static class DependencyInjection
{
    private const string DACPAC_EMBEDDED_RESOURCE_NAME = "DistributedCaching.dacpac";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("AddInfrastructure");

        var options = new DistributedCachingOptions();

        var section = configuration.GetSection(DistributedCachingOptions.SectionName);
        section.Bind(options);

        services.Configure<DistributedCachingOptions>(section);

        services.AddMediatR(Assembly.GetExecutingAssembly());

        IFileProvider fileProvider = new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly());
        services.AddSingleton<IFileProvider>(fileProvider);

        var connectionString = configuration.GetConnectionString("ConnectionString");
        logger.LogInformation(connectionString);

        logger.LogInformation(options.SchemaName);
        logger.LogInformation(options.TableName);
        logger.LogInformation(options.CacheKey);




        try
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var catalog = connectionStringBuilder.InitialCatalog;
            var dacServices = new DacServices(connectionString);
            using var stream = fileProvider.GetFileInfo(DACPAC_EMBEDDED_RESOURCE_NAME).CreateReadStream();
            using var dacPackage = DacPackage.Load(stream);
            dacServices.Deploy(dacPackage, targetDatabaseName: catalog, upgradeExisting: true, options: null, cancellationToken: null);

            services.AddDistributedSqlServerCache(cacheOptions =>
            {
                cacheOptions.ConnectionString = connectionString;
                cacheOptions.SchemaName = options.SchemaName;
                cacheOptions.TableName = options.TableName;
            });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
        }

        services.AddSingleton<IWatherForecastService, WhaterForecastFromObject>();

        return services;
    }
}
