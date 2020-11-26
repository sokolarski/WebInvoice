namespace WebInvoice.Data.Repository.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using WebInvoice.Data.Repository.Models;

    public interface IAppDeletableEntityRepository<TEntity> : IAppRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        Task<TEntity> GetByIdWithDeletedAsync(params object[] id);

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
