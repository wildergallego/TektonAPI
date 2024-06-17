using BusinessLayer.Dto;

namespace BusinessLayer.Interfaces
{
    public interface IProductRepository<TEntity>
    {
        public Task<TEntity> GetById(int idFind);

        public Task<int> AddProduct(ProductChangeDto productInsert);

        public Task<int> EditProduct(ProductChangeDto productEdit);
    }
}
