using BusinessLayer.Dto;

namespace BusinessLayer.Interfaces
{
    public interface IProductStatus
    {
        public Task<string> GetByValue(int value);
    }
}
