using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Samples;

const string LogOutput = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level} ({SourceContext})] {Scope} {Message} {Properties:j}{NewLine}{Exception}";

// Need await here to block until complete
await Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
{

}).UseSerilog((contex, config) =>
{
    // Use {Scope} and {Properties} for enrichment
    config.Enrich.FromLogContext().WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        outputTemplate: LogOutput);
    
    config.Enrich.FromLogContext()
    .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(Matching.FromSource<ServiceOne>())
        .WriteTo
        .File("Logs/serviceOne.log", LogEventLevel.Verbose,
            outputTemplate: LogOutput));
    
    config.Enrich.FromLogContext()
    .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(Matching.FromSource<ServiceTwo>())
        .WriteTo
        .File("Logs/serviceTwo.log", LogEventLevel.Verbose,
            outputTemplate: LogOutput));

}).ConfigureServices((hostContext, services) =>
{
    services.AddLogging((builder) =>
    {
        builder.AddSerilog(dispose: true);
    });
    services.AddHostedService<ServiceOne>();
    services.AddHostedService<ServiceTwo>();
}).RunConsoleAsync();
