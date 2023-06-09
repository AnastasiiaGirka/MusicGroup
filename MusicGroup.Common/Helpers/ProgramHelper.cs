using System.Security.Authentication;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using IWebHost = Microsoft.AspNetCore.Hosting.IWebHost;
using IWebHostBuilder = Microsoft.AspNetCore.Hosting.IWebHostBuilder;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace MusicGroup.Common.Helpers
{
    public static class ProgramHelper
    {
        private static LogLevel GetLogLevel(string key)
        {
            string value = Environment.GetEnvironmentVariable(key);

            if (!string.IsNullOrWhiteSpace(value))
            {
                switch (value)
                {
                    case "Information":
                        return LogLevel.Information;

                    case "Error":
                        return LogLevel.Error;

                    case "Debug":
                        return LogLevel.Debug;

                    case "Critical":
                        return LogLevel.Critical;

                    case "None":
                        return LogLevel.None;

                    case "Trace":
                        return LogLevel.Trace;

                    case "Warning":
                        return LogLevel.Warning;
                }
            }

            return LogLevel.Information;
        }

        private static void ConfigureLogging(this ILoggingBuilder builder)
        {
            LogLevel logLevel = GetLogLevel("MICROSOFT_LOGGING_LEVEL");
            builder.ClearProviders();
            builder.SetMinimumLevel(logLevel);
        }

        public static void Start<TStartup>(string[] args, bool web = true) where TStartup : class
        {
            string name = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Logger logger = NLogBuilder.ConfigureNLog("nlog.debug.config").GetCurrentClassLogger();

            try
            {
                logger.Info($"Program started. Web=\"{web}\". Environment=\"{name}\".");

                IWebHost host = CreateWebHostBuilder<TStartup>(args).Build();

                host.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, $"Stopped program because of exception. {ex.Message}");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
        
        public static IWebHostBuilder? CreateWebHostBuilder<TStartup>(string[] args) where TStartup : class
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .UseKestrel(kestrelOptions =>
                    kestrelOptions.ConfigureHttpsDefaults(httpsOptions =>
                        {
                            httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                        }
                    ))
                .UseStartup<TStartup>()
                .ConfigureAppConfiguration((hostingContext, config) => { config.AddEnvironmentVariables(); })
                .ConfigureLogging();
        }

        public static IWebHostBuilder? ConfigureLogging(this IWebHostBuilder builder)
        {
            return builder?
                .ConfigureLogging(ConfigureLogging)
                .UseNLog();
        }
    }
}