namespace BusinessLayer.Services
{
    using BusinessLayer.Common;
    using BusinessLayer.Dto;
    using BusinessLayer.Interfaces;
    using DataLayer;
    using LazyCache;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class ProductStatusService : IProductStatus
    {
        readonly IProductStatusRepository<ProductStatus> _productStatusRepository;
        readonly ProductsDbContext _context;
        readonly IAppCache _appCache;
        readonly DateTime _dueTime;

        public ProductStatusService(ProductsDbContext productsDbContext, IAppCache appCache, IProductStatusRepository<ProductStatus> repository)
        {
            this._context = productsDbContext;
            this._appCache = appCache;
            this._dueTime = DateTime.Now.AddMinutes(Constants.MINUTESCACHE);
            this._productStatusRepository = repository;
        }

        public async Task<string> GetByValue(int value)
        {
            try
            {
                var result = (IEnumerable<ProductStatusDto>) await this._appCache.GetOrAddAsync(Constants.NAMEPRODUCTSCACHE, () => (from states in _context.ProductStatuses
                                                                                                                          select new ProductStatusDto
                                                                                                                          {
                                                                                                                              Status = states.Status,
                                                                                                                              StatusName = states.StatusName
                                                                                                                          }).ToListAsync(),
                                                                                                                            this._dueTime);

                if (result == null)
                {
                    return string.Empty;
                }

                return result.Where(cond => cond.Status.Equals(value)).Select(colum => colum.StatusName).First();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
