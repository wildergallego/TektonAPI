using BusinessLayer.Dto;

namespace BusinessLayer.Interfaces
{
    public interface IProduct
    {
        public Task<ProductDto> GetById(int idFind);

        public Task<ProductDto> AddProduct(ProductChangeDto productInsert);

        public Task<ProductDto> EditProduct(ProductChangeDto productEdit);
    }
}
