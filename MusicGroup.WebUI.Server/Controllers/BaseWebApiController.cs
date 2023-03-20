using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MusicGroup.Common.Models;

namespace MusicGroup.WebUI.Server.Controllers
{
    [Authorize]
    [ApiController]
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public abstract class BaseWebApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AlbumApiClient _albumApiClient;
        private SongApiClient _songApiClient;
       
        private ILogger _logger;

        protected BaseWebApiController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            var accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            HttpContext = accessor.HttpContext;

            if (HttpContext == null)
                throw new InvalidOperationException("HttpContext is not initialized.");

            Session = HttpContext.Session;

            if (Session == null)
                throw new InvalidOperationException("Session is not initialized or not available.");
        }
        
        internal HttpContext HttpContext { get; }

        internal ISession Session { get; }

        internal IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
        }
        

        internal ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    var factory = _serviceProvider.GetRequiredService<ILoggerFactory>();
                    _logger = factory.CreateLogger(GetType());
                }

                return _logger;
            }
        }

        #region Api

        internal AlbumApiClient AlbumApiClient
        {
            get { return _albumApiClient ??= _serviceProvider.GetRequiredService<AlbumApiClient>(); }
        }
        
        internal SongApiClient SongApiClient
        {
            get { return _songApiClient ??= _serviceProvider.GetRequiredService<SongApiClient>(); }
        }
        
        #endregion
    }
}