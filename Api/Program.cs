using Api.Models;
using Api.Services;
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

        // Register your UserService
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRateRulesService, RateRulesService>();
        services.AddScoped<IOptimizerService, OptimizerService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ICommunityService, CommunityService>();
    })
    .Build();

host.Run();
