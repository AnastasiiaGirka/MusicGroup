using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicGroup.Api.Db.Context.Entities;
using MusicGroup.Api.Db.Context.Entities.Base;
using MusicGroup.Api.Db.Context.Settings;
using MusicGroup.Common;

namespace MusicGroup.Api.Db.Context
{
    public sealed class MusicGroupDbContext : DbContext
    {
        #region Private/Protected
        
        private readonly DbSettings _dbSettings;
        
        private static void CreateIndexes(ModelBuilder modelBuilder)
        {
            //TODO: define manual indexes here
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: Constants.ApplicationInMemoryDatabaseName);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            CreateIndexes(modelBuilder);
        }

        #endregion
        

        public MusicGroupDbContext(DbContextOptions<MusicGroupDbContext> options, IOptions<DbSettings> dbSettings, Func<BaseDbEntity, Task> onChangeAsync)
            : base(options)
        {
            if (dbSettings == null)
                throw new ArgumentNullException(nameof(dbSettings));
            OnChangeAsync = onChangeAsync;

            _dbSettings = dbSettings.Value;
        }

        public MusicGroupDbContext(IOptions<DbSettings> config, DbSettings dbSettings, Func<BaseDbEntity, Task> onChangeAsync)
        {
            OnChangeAsync = onChangeAsync;
            _dbSettings = config.Value;
        }

        public MusicGroupDbContext(IOptions<DbSettings> config)
        {
            _dbSettings = config.Value;
        }
        
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new InvalidOperationException($"Only async operations are supported for \"{nameof(MusicGroupDbContext)}\". Please use \"{nameof(SaveChangesAsync)}\" instead of \"{nameof(SaveChanges)}\".");
        }
        
        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BaseDbEntity[] entities = null;

            int result = await base.SaveChangesAsync(true, cancellationToken);

            if ((OnChangeAsync != null) && (entities != null) && (entities.Length > 0))
            {
                foreach (BaseDbEntity entity in entities)
                {
                    await OnChangeAsync(entity);
                }
            }

            return result;
        }

        public Func<BaseDbEntity, Task> OnChangeAsync { get; set; }

        #region Tables
        
        public DbSet<AlbumDbEntity> Albums { get; set; }
        
        public DbSet<SongDbEntity> Songs { get; set; }
        
        #endregion

        #region Constants

        public static readonly Version MySqlVersion = new Version(5, 7, 17);

        public static readonly string MigrationsAssemblyName = typeof(MusicGroupDbContext).Assembly.GetName().Name;
      
        #endregion Constants
    }
}