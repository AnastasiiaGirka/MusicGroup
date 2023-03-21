using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using MusicGroup.Api.Db;
using MusicGroup.Api.Db.Context;
using MusicGroup.Api.Db.FileDb;
using MusicGroup.Common;
using MusicGroup.Server.Services;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace MusicGroup;

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

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = Constants.ApplicationName, Version = "v1" }); });
    }

    private static void AddMvc(IServiceCollection services)
    {
        services
            .AddMvc(option => option.EnableEndpointRouting = false)
            .AddNewtonsoftJson();
    }

    // This method gets called by the runtime. Use this method to add services  to the container.
    public static void ConfigureServices(IServiceCollection services)
    {
        AddSwagger(services);

        AddDb(services);

        AddServices(services);

        services.AddHealthChecks();

        services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder.WithOrigins("http://localhost")
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        AddMvc(services);
    }

    private static void AddDb(IServiceCollection services)
    {
        string connectionString = $"server=localhost;database={Constants.ApplicationInMemoryDatabaseName};";

        services.AddDbContext<MusicGroupDbContext>(options =>
            options.UseMySql(connectionString,
                new MySqlServerVersion(MusicGroupDbContext.MySqlVersion),
                mysqlOptions =>
                {
                    mysqlOptions.EnableStringComparisonTranslations();
                    mysqlOptions.MigrationsAssembly(MusicGroupDbContext.MigrationsAssemblyName);
                }
            ));

        services.AddScoped<DbService>();
        services.AddScoped<FileDb>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<AlbumService>();
        services.AddScoped<SongService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", Constants.ApplicationName);
            c.RoutePrefix = string.Empty;
        });

        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            // WriteResponse is a delegate used to write the response.
            ResponseWriter = WriteResponse
        });

        app.UseAuthentication();

        // ReSharper disable once MVC1005
#pragma warning disable MVC1005
        app.UseMvc();
#pragma warning restore MVC1005

        Logger.LogInformation($"Startup Api.ServiceHost on \"{Environment.MachineName}\" initialized.");
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

        return httpContext.Response.WriteAsync(json.ToString((Formatting)System.Xml.Formatting.Indented));
    }
}