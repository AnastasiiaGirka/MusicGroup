using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MusicGroup.Api.Db;

namespace MusicGroup.Server.Services
{
    public abstract class BaseService
    {
        private readonly IServiceProvider _serviceProvider;
        private AlbumService _albumService;
        private ILogger _logger;
        private DbService _dbService;

        protected BaseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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

        #region Services

        protected AlbumService AlbumService => _albumService ??= _serviceProvider.GetRequiredService<AlbumService>();
        
        protected DbService DbService => _dbService ??= _serviceProvider.GetRequiredService<DbService>();

        #endregion

        protected IServiceProvider ServiceProvider => _serviceProvider;
        
    }
}