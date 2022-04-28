using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StatsdClient;

namespace Samples
{
    public class ServiceOne : IHostedService
    {
        public ServiceOne(ILogger<ServiceOne> logger, IHostApplicationLifetime appLifetime, IDogStatsd metrics)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.metrics = metrics;
            this.appLifetime = appLifetime;
            this.logger = logger;
        }

        private int? exitCode;
        private readonly ILogger<ServiceOne> logger;
        private readonly IHostApplicationLifetime appLifetime;
        private readonly IDogStatsd metrics;

        private void Run()
        {
            try
            {
                var i = 0;
                while(true)
                {
                    this.logger.LogInformation("Loop"); 
                    using (this.metrics.StartTimer("dev.sample.timer"))
                    {

                        System.Threading.Thread.Sleep(10000); 
                    }
                    i++;
                    this.metrics.Counter("dev.sample.count", i);
                    this.metrics.Event("Test", "Sample Event");
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

    public class StructuredData
    {
        // Only public properties get serialized and show up in the logs.
        public string PublicStringProperty { get; set; }
        public int PublicIntProperty { get; set; }

        // The following properties/fields will not show up in the logs.
        private string PrivateStringProperty { get; set; }

        public string PublicStringField;
        private string PrivateStringField;

        public StructuredData()
        {
            PublicStringProperty = "Public property value";
            PublicIntProperty = 1;
            PrivateStringProperty = "Private property value";
            PublicStringField = "Public field value";
            PrivateStringField = "Private field value";
        }
    }
}