using BusinessLayer.Dto;

namespace BusinessLayer.Interfaces
{
    public interface IProductStatusRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> Get();
    }
}
