using Api.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Telemetry
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // EF Core DbContext with SQL Server
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<SmartForexDBContext>(options =>
            options.UseSqlServer(connectionString));
    })
    .Build();

host.Run();
