using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Domain.Contracts;
using ArdaManager.Infrastructure.Contexts;
using LazyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Repositories
{
    public class UnitOfWork<TId> : IUnitOfWork<TId>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly VappContext _dbContext;
        private readonly IAppCache _cache;
        private bool _disposed;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(VappContext dbContext, ICurrentUserService currentUserService, IAppCache cache)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repositories = new Dictionary<Type, object>();
        }

        public IRepositoryAsync<TEntity, TId> Repository<TEntity>() where TEntity : AuditableEntity<TId>
        {
            var entityType = typeof(TEntity);
            if (!_repositories.ContainsKey(entityType))
            {
                _repositories[entityType] = new RepositoryAsync<TEntity, TId>(_dbContext);
            }
            return (IRepositoryAsync<TEntity, TId>)_repositories[entityType];
        }

        public IVappRepository VappRepository => _vappRepository ??= new VappRepository(_dbContext);

        private IVappRepository _vappRepository;

        public async Task<int> Commit(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken = default, params string[] cacheKeys)
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            foreach (var cacheKey in cacheKeys)
            {
                _cache.Remove(cacheKey);
            }
            return result;
        }

        public Task Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _dbContext.Dispose();
                    //_vappRepository?.Dispose();
                }
                // Dispose unmanaged resources
                _disposed = true;
            }
        }
    }
}