// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Samples;
using StatsdClient;


await Host.CreateDefaultBuilder(args).ConfigureLogging(x =>
{

}).ConfigureAppConfiguration((hostingContext, config) =>
{
    
}).ConfigureServices((hostContext, services) =>
{
    // The client can use the DD_AGENT_HOST and (optionally) the DD_DOGSTATSD_PORT environment variables to build the target address if the StatsdServerName and/or StatsdPort parameters are empty.
    var dogstatsdConfig = new StatsdConfig
    {
        //StatsdServerName = "127.0.0.1",
        //StatsdPort = 8125,
    };

    var dogsService = new DogStatsdService();
    dogsService.Configure(dogstatsdConfig);
    
    services.AddSingleton<IDogStatsd>(x => dogsService);
    services.AddLogging(x => x.AddConsole());
    services.AddHostedService<ServiceOne>();
    services.AddHostedService<ServiceTwo>();
}).RunConsoleAsync();
