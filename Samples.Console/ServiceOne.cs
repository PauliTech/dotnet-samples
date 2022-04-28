using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SerilogTimings;

namespace Serilog.Samples
{
    public class ServiceOne : IHostedService
    {
        public ServiceOne(ILogger<ServiceOne> logger, IHostApplicationLifetime appLifetime)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.appLifetime = appLifetime;
            this.logger = logger;
        }

        private int? exitCode;
        private readonly ILogger<ServiceOne> logger;
        private readonly IHostApplicationLifetime appLifetime;

        private void Run()
        {
            try
            {
                // Loop
                var structuredData = new StructuredData();
                var simpleData = "This is a string.";

                // Use the static Serilog.Log class for logging.
                this.logger.LogTrace("Here's a Trace message.");
                this.logger.LogDebug("Here's a Debug message. Only Public Properties (not fields) are shown on structured data. Structured data: {@sampleData}. Simple data: {simpleData}.", structuredData, simpleData);
                this.logger.LogInformation(new Exception("Exceptions can be put on all log levels"), "Here's an Info message.");
                this.logger.LogWarning("Here's a Warning message.");
                this.logger.LogError(new Exception("This is an exception."), "Here's an Error message.");
                this.logger.LogCritical("Here's a Critical message.");

                var scopeProps = new Dictionary<string, object> {
                    { "TransactionId", 12345 },
                    { "ResponseJson", System.Text.Json.JsonSerializer.Serialize(structuredData) },
                };
                using (this.logger.BeginScope(scopeProps))
                {
                    this.logger.LogInformation("Transaction completed in {DurationMs}ms...", 30);
                }
                // 2022-04-21 15:48:52.181 [Information] (Serilog.Samples.ServiceOne)  Transaction completed in 30ms... {TransactionId=12345, ResponseJson="{\"PublicStringProperty\":\"Public property value\",\"PublicIntProperty\":1}"}     
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