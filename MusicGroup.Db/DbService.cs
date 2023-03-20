using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using MusicGroup.Api.Db.Context;
using MusicGroup.Api.Db.Context.Entities.Base;
using MusicGroup.Api.Db.Interfaces;

namespace MusicGroup.Api.Db
{
    public partial class DbService
    {
        private readonly MusicGroupDbContext _context;
        private readonly ILogger<DbService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DbService(IServiceProvider serviceProvider, MusicGroupDbContext context, ILogger<DbService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;

            context.OnChangeAsync = OnChangeAsync;
        }

        private async Task OnChangeAsync(BaseDbEntity entity)
        {
            //TODO: Implement
            await Task.FromResult(0);
        }

        private async Task SaveAsync<TDbEntity>(TDbEntity entity, Func<TDbEntity, Task<int>> getNumberAsync, Action<TDbEntity> addCallback = null) where TDbEntity : BaseDbEntity, INumerableDbEntity
        {
            EntityEntry<TDbEntity> entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                if (entity.Number == 0)
                {
                    entity.Number = await getNumberAsync(entity);
                }

                await _context.AddAsync(entity);

                addCallback?.Invoke(entity);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex) when ((ex.InnerException != null) && (ex.InnerException.Message.Contains("IX_")) && (ex.InnerException.Message.Contains("_Number")))
                {
                    entity.Number = 0;
                    entry.State = EntityState.Detached;
                    await SaveAsync(entity, getNumberAsync, addCallback);
                }
            }
            else
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync<TDbEntity>(TDbEntity entity) where TDbEntity : BaseDbEntity
        {
            EntityEntry entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                await _context.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task HealthCheckAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task SaveChangesAsync(bool auditTracking = true)
        {
            await _context.SaveChangesAsync(auditTracking);
        }

        public async Task SaveAsync(bool auditTracking = true)
        {
            await SaveChangesAsync(auditTracking);
        }

        public async Task AddAsync(BaseDbEntity entity, bool saveChanges = true, bool auditTracking = true)
        {
            await _context.AddAsync(entity);

            if (saveChanges)
            {
                await _context.SaveChangesAsync(auditTracking);
            }
        }

        public async Task AddAsync(IEnumerable<BaseDbEntity> entities, bool saveChanges = true, bool auditTracking = true)
        {
            await _context.AddRangeAsync(entities);

            if (saveChanges)
            {
                await _context.SaveChangesAsync(auditTracking);
            }
        }
    }
}