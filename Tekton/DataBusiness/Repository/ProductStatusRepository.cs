using BusinessLayer.Common;
using BusinessLayer.Dto;
using BusinessLayer.Interfaces;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repository
{
    public class ProductStatusRepository : IProductStatusRepository<ProductStatus>
    {
        readonly ProductsDbContext _context;

        public ProductStatusRepository(ProductsDbContext productsDbContext)
        {
            _context = productsDbContext;
        }

        public async Task<IEnumerable<ProductStatus>> Get()
        {
            return await _context.ProductStatuses.ToListAsync();
        }
    }
}
