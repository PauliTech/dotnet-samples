namespace Samples.CommandBus;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


public class ServiceOne : IHostedService
{
    public ServiceOne(ILogger<ServiceOne> logger, IHostApplicationLifetime appLifetime, ICommandBus commandBus)
    {
        this.logger = logger;
        this.appLifetime = appLifetime;
        this.commandBus = commandBus;
        this.logger = logger;
    }

    private int? exitCode;
    private readonly ILogger<ServiceOne> logger;
    private readonly IHostApplicationLifetime appLifetime;
    private readonly ICommandBus commandBus;

    private void Run()
    {
        try
        {
            System.Console.WriteLine("Sample of the command bus pattern in C#");
            while (true)
            {
                var addition = new AdditionCommand() { Addend = 5, Augend = 5 };
                var result = this.commandBus.Execute<AdditionCommand, MathReturnValue>(addition);
                var message = $"5 + 5 = {result.Result}";

                var output = WriteToOutputterCommand.Create(message);
                this.commandBus.Execute(output);

                System.Threading.Thread.Sleep(10000);
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unhandled exception!");
            this.exitCode = 1;
        }
        finally
        {

        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () => this.Run());
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogDebug($"Exiting with return code: {exitCode}");
        Environment.ExitCode = exitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }
}
