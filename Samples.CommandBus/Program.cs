// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Samples;
using Samples.CommandBus;

await Host.CreateDefaultBuilder(args).ConfigureLogging(x =>
{

}).ConfigureAppConfiguration((hostingContext, config) =>
{

}).ConfigureServices((hostContext, services) =>
{
    // Set up of the command handlers and the bus in the service configuration.
    // This is fine as it allows classes to have the handler injected.
    services.AddTransient<ICommandHandler<AdditionCommand, MathReturnValue>, AdditionCommandHandler>();
    services.AddTransient<ICommandHandler<WriteToOutputterCommand>, WriteToOutputterCommandHandler>();
    services.AddTransient<ITextOutputter, ConsoleTextOutputter>();
    services.AddSingleton<ICommandBus, CommandBus>(x =>
    {
        var bus = new CommandBus();
        // Is having a class that contains all the handlers a anti-pattern?
        bus.Register(x.GetService<ICommandHandler<AdditionCommand, MathReturnValue>>()!);
        bus.Register(x.GetService<ICommandHandler<WriteToOutputterCommand>>()!);
        return bus;
    });


    services.AddLogging(x => x.AddConsole());
    services.AddHostedService<ServiceOne>();
}).RunConsoleAsync();