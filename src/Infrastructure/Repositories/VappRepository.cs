﻿using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Contracts;
using ArdaManager.Infrastructure.Contexts;
using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Repositories
{
    /// <summary>
    /// This class includes implementations of repository functions
    /// </summary>
    internal class VappRepository : IVappRepository
    {
        private readonly VappContext context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">
        /// Database Context <see cref="DbContext"/>
        /// </param>
        public VappRepository(VappContext context)
        {
            this.context = context;
        }

        public virtual TEntity Add<TEntity>(TEntity entity) where TEntity : class, new()
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual TEntity Add<TEntity, TPrimaryKey>(TEntity entity) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entity.CreatedOn = DateTime.UtcNow;
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual async Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            await context.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public virtual async Task<TEntity> AddAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entity.CreatedOn = DateTime.UtcNow;
            await context.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public virtual IEnumerable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            context.Set<TEntity>().AddRange(entities);
            context.SaveChanges();
            return entities;
        }

        public virtual IEnumerable<TEntity> AddRange<TEntity, TPrimaryKey>(IEnumerable<TEntity> entities) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entities.ToList().ForEach(x => x.CreatedOn = DateTime.UtcNow);
            context.Set<TEntity>().AddRange(entities);
            context.SaveChanges();
            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            await context.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync<TEntity, TPrimaryKey>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entities.ToList().ForEach(x => x.CreatedOn = DateTime.UtcNow);
            await context.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entities;
        }

        public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>> anyExpression) where TEntity : class, new()
        {
            return context.Set<TEntity>().Any(anyExpression);
        }

        public virtual async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> anyExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            bool result = await context.Set<TEntity>().AnyAsync(anyExpression, cancellationToken).ConfigureAwait(false);
            return result;
        }

        public virtual int Count<TEntity>() where TEntity : class, new()
        {
            return context.Set<TEntity>().Count();
        }

        public virtual int Count<TEntity>(Expression<Func<TEntity, bool>> whereExpression) where TEntity : class, new()
        {
            return context.Set<TEntity>().Where(whereExpression).Count();
        }

        public virtual async Task<int> Count<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            int count = await context.Set<TEntity>().Where(whereExpression).CountAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public virtual int Count<TEntity, TFilter>(TFilter filter)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            return context.Set<TEntity>().ApplyFilter(filter).Count();
        }

        public virtual async Task<int> CountAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            int count = await context.Set<TEntity>().CountAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public virtual async Task<int> CountAsync<TEntity, TFilter>(TFilter filter, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            int count = await context.Set<TEntity>().ApplyFilter(filter).CountAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        private Expression<Func<TEntity, bool>> GenerateExpression<TEntity>(object id)
        {
            var type = context.Model.FindEntityType(typeof(TEntity));
            string pk = type.FindPrimaryKey().Properties.Select(s => s.Name).FirstOrDefault();
            Type pkType = type.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            object value = Convert.ChangeType(id, pkType, CultureInfo.InvariantCulture);

            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entity");
            MemberExpression me = Expression.Property(pe, pk);
            ConstantExpression constant = Expression.Constant(value, pkType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<TEntity, bool>> expression = Expression.Lambda<Func<TEntity, bool>>(body, new[] { pe });

            return expression;
        }

        public virtual void HardDelete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public virtual void HardDelete<TEntity>(object id) where TEntity : class, new()
        {
            var entity = context.Set<TEntity>().Find(id);
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public virtual void HardDelete<TEntity, TPrimaryKey>(TEntity entity) where TEntity : AuditableEntity<TPrimaryKey>
        {
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public virtual void HardDelete<TEntity, TPrimaryKey>(TPrimaryKey id) where TEntity : AuditableEntity<TPrimaryKey>
        {
            var entity = context.Set<TEntity>().FirstOrDefault(GenerateExpression<TEntity>(id));
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public virtual async Task HardDeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task HardDeleteAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(GenerateExpression<TEntity>(id), cancellationToken).ConfigureAwait(false);
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task HardDeleteAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task HardDeleteAsync<TEntity, TPrimaryKey>(TPrimaryKey id, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(GenerateExpression<TEntity>(id), cancellationToken).ConfigureAwait(false);
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

        public virtual TEntity Replace<TEntity>(TEntity entity) where TEntity : class, new()
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return entity;
        }

        public virtual TEntity Replace<TEntity, TPrimaryKey>(TEntity entity) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entity.LastModifiedOn = DateTime.UtcNow;
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return entity;
        }

        public virtual async Task<TEntity> ReplaceAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public virtual async Task<TEntity> ReplaceAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entity.LastModifiedOn = DateTime.UtcNow;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }
        /*
        public virtual void SoftDelete<TEntity, TPrimaryKey>(TEntity entity) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entity.IsDeleted = true;
            entity.DeletionDate = DateTime.UtcNow;
            Replace<TEntity, TPrimaryKey>(entity);
        }

        public virtual void SoftDelete<TEntity, TPrimaryKey>(TPrimaryKey id) where TEntity : AuditableEntity<TPrimaryKey>
        {
            var entity = context.Set<TEntity>().FirstOrDefault(GenerateExpression<TEntity>(id));
            entity.IsDeleted = true;
            entity.DeletionDate = DateTime.UtcNow;
            Replace<TEntity, TPrimaryKey>(entity);
        }

        public virtual async Task SoftDeleteAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entity.IsDeleted = true;
            entity.DeletionDate = DateTime.UtcNow;
            await ReplaceAsync<TEntity, TPrimaryKey>(entity, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task SoftDeleteAsync<TEntity, TPrimaryKey>(TPrimaryKey id, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(GenerateExpression<TEntity>(id), cancellationToken).ConfigureAwait(false); ;
            entity.IsDeleted = true;
            entity.DeletionDate = DateTime.UtcNow;
            await ReplaceAsync<TEntity, TPrimaryKey>(entity, cancellationToken).ConfigureAwait(false);
        }
        */
        public virtual TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            context.Set<TEntity>().Update(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual TEntity Update<TEntity, TPrimaryKey>(TEntity entity) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entity.LastModifiedOn = DateTime.UtcNow;
            context.Set<TEntity>().Update(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entity.LastModifiedOn = DateTime.UtcNow;
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public virtual IEnumerable<TEntity> UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            context.Set<TEntity>().UpdateRange(entities);
            context.SaveChanges();
            return entities;
        }

        public virtual IEnumerable<TEntity> UpdateRange<TEntity, TPrimaryKey>(IEnumerable<TEntity> entities) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entities.ToList().ForEach(a => a.LastModifiedOn = DateTime.UtcNow);
            context.Set<TEntity>().UpdateRange(entities);
            context.SaveChanges();
            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            context.Set<TEntity>().UpdateRange(entities);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> UpdateRangeAsync<TEntity, TPrimaryKey>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            entities.ToList().ForEach(a => a.LastModifiedOn = DateTime.UtcNow);
            context.Set<TEntity>().UpdateRange(entities);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entities;
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, new()
        {
            return context.Set<TEntity>().AsQueryable();
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class, new()
        {
            return context.Set<TEntity>().Where(filter);
        }

        private IQueryable<TEntity> FindQueryable<TEntity>(bool asNoTracking) where TEntity : class, new()
        {
            var queryable = GetQueryable<TEntity>();
            if (asNoTracking)
            {
                queryable = queryable.AsNoTracking();
            }
            return queryable;
        }

        public virtual List<TEntity> GetMultiple<TEntity>(bool asNoTracking) where TEntity : class, new()
        {
            return FindQueryable<TEntity>(asNoTracking).ToList();
        }

        public virtual async Task<List<TEntity>> GetMultipleAsync<TEntity>(bool asNoTracking, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await FindQueryable<TEntity>(asNoTracking).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TProjected> GetMultiple<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, TProjected>> projectExpression) where TEntity : class, new()
        {
            return FindQueryable<TEntity>(asNoTracking).Select(projectExpression).ToList();
        }

        public virtual async Task<List<TProjected>> GetMultipleAsync<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, TProjected>> projectExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await FindQueryable<TEntity>(asNoTracking).Select(projectExpression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TEntity> GetMultiple<TEntity>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression) where TEntity : class, new()
        {
            return FindQueryable<TEntity>(asNoTracking).Where(whereExpression).ToList();
        }

        public virtual async Task<List<TEntity>> GetMultipleAsync<TEntity>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await FindQueryable<TEntity>(asNoTracking).Where(whereExpression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TProjected> GetMultiple<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProjected>> projectExpression) where TEntity : class, new()
        {
            return FindQueryable<TEntity>(asNoTracking).Where(whereExpression).Select(projectExpression).ToList();
        }

        public virtual async Task<List<TProjected>> GetMultipleAsync<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProjected>> projectExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await FindQueryable<TEntity>(asNoTracking).Where(whereExpression).Select(projectExpression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TEntity> GetMultiple<TEntity>(bool asNoTracking, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking);
            queryable = includeExpression(queryable);
            return queryable.ToList();
        }

        public virtual async Task<List<TEntity>> GetMultipleAsync<TEntity>(bool asNoTracking, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking);
            queryable = includeExpression(queryable);
            return await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TEntity> GetMultiple<TEntity>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            queryable = includeExpression(queryable);
            return queryable.ToList();
        }

        public virtual async Task<List<TEntity>> GetMultipleAsync<TEntity>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            queryable = includeExpression(queryable);
            return await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TProjected> GetMultiple<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, Expression<Func<TEntity, TProjected>> projectExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            queryable = includeExpression(queryable);
            return queryable.Select(projectExpression).ToList();
        }

        public virtual async Task<List<TProjected>> GetMultipleAsync<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, Expression<Func<TEntity, TProjected>> projectExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            queryable = includeExpression(queryable);
            return await queryable.Select(projectExpression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TEntity> GetMultiple<TEntity, TFilter>(bool asNoTracking, TFilter filter)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            return FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter).ToList();
        }

        public virtual async Task<List<TEntity>> GetMultipleAsync<TEntity, TFilter>(bool asNoTracking, TFilter filter, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            return await FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TEntity> GetMultiple<TEntity, TFilter>(bool asNoTracking, TFilter filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            queryable = includeExpression(queryable);
            return queryable.ToList();
        }

        public virtual async Task<List<TEntity>> GetMultipleAsync<TEntity, TFilter>(bool asNoTracking, TFilter filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            queryable = includeExpression(queryable);
            return await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TProjected> GetMultiple<TEntity, TFilter, TProjected>(bool asNoTracking, TFilter filter, Expression<Func<TEntity, TProjected>> projectExpression)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            return FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter).Select(projectExpression).ToList();
        }

        public virtual async Task<List<TProjected>> GetMultipleAsync<TEntity, TFilter, TProjected>(bool asNoTracking, TFilter filter, Expression<Func<TEntity, TProjected>> projectExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            return await FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter).Select(projectExpression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual List<TProjected> GetMultiple<TEntity, TFilter, TProjected>(bool asNoTracking, TFilter filter, Expression<Func<TEntity, TProjected>> projectExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            queryable = includeExpression(queryable);
            return queryable.Select(projectExpression).ToList();
        }

        public virtual async Task<List<TProjected>> GetMultipleAsync<TEntity, TFilter, TProjected>(bool asNoTracking, TFilter filter, Expression<Func<TEntity, TProjected>> projectExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            queryable = includeExpression(queryable);
            return await queryable.Select(projectExpression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TEntity GetSingle<TEntity>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            return queryable.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetSingleAsync<TEntity>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            return await queryable.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TEntity GetSingle<TEntity>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            queryable = includeExpression(queryable);
            return queryable.FirstOrDefault();
        }

        public async virtual Task<TEntity> GetSingleAsync<TEntity>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            queryable = includeExpression(queryable);
            return await queryable.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TProjected GetSingle<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProjected>> projectExpression) where TEntity : class, new()
        {
            return FindQueryable<TEntity>(asNoTracking).Where(whereExpression).Select(projectExpression).FirstOrDefault();
        }

        public virtual async Task<TProjected> GetSingleAsync<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProjected>> projectExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await FindQueryable<TEntity>(asNoTracking).Where(whereExpression).Select(projectExpression).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TProjected GetSingle<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProjected>> projectExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            queryable = includeExpression(queryable);
            return queryable.Select(projectExpression).FirstOrDefault();
        }

        public virtual async Task<TProjected> GetSingleAsync<TEntity, TProjected>(bool asNoTracking, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProjected>> projectExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(whereExpression);
            queryable = includeExpression(queryable);
            return await queryable.Select(projectExpression).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TEntity GetSingle<TEntity, TFilter>(bool asNoTracking, TFilter filter)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            return queryable.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetSingleAsync<TEntity, TFilter>(bool asNoTracking, TFilter filter, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            return await queryable.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TEntity GetSingle<TEntity, TFilter>(bool asNoTracking, TFilter filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            queryable = includeExpression(queryable);
            return queryable.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetSingleAsync<TEntity, TFilter>(bool asNoTracking, TFilter filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            queryable = includeExpression(queryable);
            return await queryable.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TProjected GetSingle<TEntity, TProjected, TFilter>(bool asNoTracking, TFilter filter, Expression<Func<TEntity, TProjected>> projectExpression)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            return queryable.Select(projectExpression).FirstOrDefault();
        }

        public virtual async Task<TProjected> GetSingleAsync<TEntity, TProjected, TFilter>(bool asNoTracking, TFilter filter, Expression<Func<TEntity, TProjected>> projectExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            return await queryable.Select(projectExpression).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TProjected GetSingle<TEntity, TProjected, TFilter>(bool asNoTracking, TFilter filter, Expression<Func<TEntity, TProjected>> projectExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            queryable = includeExpression(queryable);
            return queryable.Select(projectExpression).FirstOrDefault();
        }

        public virtual async Task<TProjected> GetSingleAsync<TEntity, TProjected, TFilter>(bool asNoTracking, TFilter filter, Expression<Func<TEntity, TProjected>> projectExpression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TFilter : FilterBase
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).ApplyFilter(filter);
            queryable = includeExpression(queryable);
            return await queryable.Select(projectExpression).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TEntity GetById<TEntity>(bool asNoTracking, object id) where TEntity : class, new()
        {
            return context.Set<TEntity>().FirstOrDefault(GenerateExpression<TEntity>(id));
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(bool asNoTracking, object id, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await FindQueryable<TEntity>(asNoTracking).FirstOrDefaultAsync(GenerateExpression<TEntity>(id), cancellationToken).ConfigureAwait(false);
        }

        public virtual TEntity GetById<TEntity>(bool asNoTracking, object id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(GenerateExpression<TEntity>(id));
            queryable = includeExpression(queryable);
            return queryable.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(bool asNoTracking, object id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(GenerateExpression<TEntity>(id));
            queryable = includeExpression(queryable);
            return await queryable.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TProjected GetById<TEntity, TProjected>(bool asNoTracking, object id, Expression<Func<TEntity, TProjected>> projectExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(GenerateExpression<TEntity>(id));
            return queryable.Select(projectExpression).FirstOrDefault();
        }

        public virtual async Task<TProjected> GetByIdAsync<TEntity, TProjected>(bool asNoTracking, object id, Expression<Func<TEntity, TProjected>> projectExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(GenerateExpression<TEntity>(id));
            return await queryable.Select(projectExpression).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual TProjected GetById<TEntity, TProjected>(bool asNoTracking, object id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, Expression<Func<TEntity, TProjected>> projectExpression) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(GenerateExpression<TEntity>(id));
            queryable = includeExpression(queryable);
            return queryable.Select(projectExpression).FirstOrDefault();
        }

        public virtual async Task<TProjected> GetByIdAsync<TEntity, TProjected>(bool asNoTracking, object id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression, Expression<Func<TEntity, TProjected>> projectExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var queryable = FindQueryable<TEntity>(asNoTracking).Where(GenerateExpression<TEntity>(id));
            queryable = includeExpression(queryable);
            return await queryable.Select(projectExpression).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public void SoftDelete<TEntity, TPrimaryKey>(TEntity entity) where TEntity : AuditableEntity<TPrimaryKey>
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            throw new NotImplementedException();
        }

        public void SoftDelete<TEntity, TPrimaryKey>(TPrimaryKey id) where TEntity : AuditableEntity<TPrimaryKey>
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync<TEntity, TPrimaryKey>(TPrimaryKey id, CancellationToken cancellationToken = default) where TEntity : AuditableEntity<TPrimaryKey>
        {
            throw new NotImplementedException();
        }


        public async virtual Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, params SqlParameter[] parameters) where T : class, new()
        {
            return await context.Set<T>().FromSqlRaw($"EXECUTE {storedProcedureName} {string.Join(", ", parameters.Select(p => p.ParameterName))}", parameters).ToListAsync();
        }


        public async Task<List<T>> ExecuteViewAsync<T>(string viewName) where T : class, new()
        {
            return await context.Set<T>().FromSqlRaw($"SELECT * FROM {viewName}").ToListAsync();
        }

    }
}
