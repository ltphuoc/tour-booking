using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        //async
        Task InsertAsync(TEntity entity);

        Task InsertRangeAsync(IQueryable<TEntity> entities);

        DbSet<TEntity> GetAll();
        IQueryable<TEntity> GetAllApart();
        Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> FindAll(Func<TEntity, bool> predicate);

        TEntity Find(Func<TEntity, bool> predicate);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetById(int Id);

        Task<TEntity> GetByIdGuid(Guid Id);

        void Insert(TEntity entity);

        Task Update(TEntity entity, int Id);

        Task UpdateGuid(TEntity entity, Guid Id);

        void UpdateRange(IQueryable<TEntity> entities);

        Task HardDelete(int key);

        Task HardDeleteGuid(Guid key);

        void DeleteRange(IQueryable<TEntity> entities);

        void InsertRange(IQueryable<TEntity> entities);

        public EntityEntry<TEntity> Delete(TEntity entity);
        public Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);

        public Task UpdateDetached(TEntity entity);
        public Task DetachEntity(TEntity entity);
        public IQueryable<TEntity> AsNoTracking();
    }
}
