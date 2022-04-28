using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Serilog.Samples
{
    public class ServiceTwo : IHostedService
    {
        public ServiceTwo(ILogger<ServiceTwo> logger, IHostApplicationLifetime appLifetime)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.logger = logger;
        }

        private int? exitCode;
        private readonly ILogger<ServiceTwo> logger;
        private readonly IHostApplicationLifetime appLifetime;

        private void Run()
        {
            try
            {
                using (this.logger.BeginScope("open scope"))
                {
                    this.logger.LogTrace("Here's a Trace message.");
                    this.logger.LogInformation(new Exception("Exceptions can be put on all log levels"), "Here's an Info message.");
                    this.logger.LogWarning("Here's a Warning message.");
                    this.logger.LogError(new Exception("This is an exception."), "Here's an Error message.");
                    this.logger.LogCritical("Here's a Critical message.");
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

            this.appLifetime.ApplicationStopping.Register(() =>
            {
                return;
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
}