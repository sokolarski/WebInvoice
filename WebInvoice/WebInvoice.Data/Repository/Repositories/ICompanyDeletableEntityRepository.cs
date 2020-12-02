using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.Repository.Repositories
{
    public interface ICompanyDeletableEntityRepository<TEntity> : ICompanyRepository<TEntity>
         where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        Task<TEntity> GetByIdWithDeletedAsync(params object[] id);

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
