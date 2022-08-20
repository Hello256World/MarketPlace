using MarketPlace.DataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Repository
{
    public interface IGenericRepository<TEntity> :IAsyncDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetQuery();
        Task AddEntity(TEntity entity);
        Task<TEntity> GetEntityById(long entityid);
        void EditeEntity(TEntity entity);
        void DeleteEntity(TEntity entity);
        Task DeleteEntity(long entityid);
        void DeletePermanent(TEntity entity);
        Task DeletePermanent(long entityid);
        Task SaveChanges();
    }
}
