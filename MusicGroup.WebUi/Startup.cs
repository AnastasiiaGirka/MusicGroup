using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using MusicGroup.Common;
using MusicGroup.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace MusicGroup.WebUi;

public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IConfiguration Configuration { get; }

        // ReSharper disable once MemberCanBePrivate.Global
        public ILogger<Startup> Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddOptions();

            ConfigureCookies(services);

            AddApiClients(services);

            AddSpa(services);

            services.AddHealthChecks();

            AddMvc(services);

            services.AddSession();
        }

        private static void ConfigureCookies(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        private static void AddApiClient<TClient>(IServiceCollection services, Uri endpoint) where TClient : class
        {
            services.AddHttpClient<TClient>(client => { client.BaseAddress = endpoint; });
        }

        private void AddApiClients(IServiceCollection services)
        {
            var endpoint = new Uri(@"http://localhost:38080/");

            AddApiClient<AlbumApiClient>(services, endpoint);
            AddApiClient<SongApiClient>(services, endpoint);
        }

        private static void AddSpa(IServiceCollection services)
        {
          services.AddSpaStaticFiles(spaConfiguration => { spaConfiguration.RootPath = "./build"; });
        }

        private static void AddMvc(IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.HeaderName = Constants.XsrfTokenName;
                options.SuppressXFrameOptionsHeader = false;
            });

            services
                .AddMvc(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRequestLocalization();

            app.UseSpaStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = responseContext =>
                {
                    // Cache all static resources for 1 year (versioned filenames)
                    ResponseHeaders headers = responseContext.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(365)
                    };
                }
            });

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                // WriteResponse is a delegate used to write the response.
                ResponseWriter = WriteResponse
            });

            app.UseSession();

            app.UseAuthentication();

#pragma warning disable MVC1005
            app.UseMvc();
            #pragma warning restore MVC1005

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = ".";

                spa.UseAngularCliServer("start");
            });

            Logger.LogInformation($"Startup WebUI on \"{Environment.MachineName}\" initialized.");
        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = Constants.Http.ApiContextType;

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
