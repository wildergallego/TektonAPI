using BusinessLayer.Services;

namespace BusinessLayer.Interfaces
{
    public interface IProductDiscount
    {
        public Task<int> GetDiscountById(int idProduct);
    }
}
