using BusinessLayer.Common;
using BusinessLayer.Dto;
using BusinessLayer.Interfaces;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repository
{
    public class ProductRepository : IProductRepository<Product>
    {
        readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext productsDbContext)
        {
            _context = productsDbContext;
        }

        public async Task<Product> GetById(int idFind)
        {
            return await _context.Products.Where(item => item.ProductId.Equals(idFind)).FirstAsync();
        }

        public async Task<int> AddProduct(ProductChangeDto productInsert)
        {
            var newProduct = new DataLayer.Product
            {
                Name = productInsert.Name,
                Status = productInsert.Status,
                Stock = productInsert.Stock,
                Description = productInsert.Description,
                Price = productInsert.Price
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return newProduct.ProductId;
        }

        public async Task<int> EditProduct(ProductChangeDto productEdit)
        {
            var productFound = await _context.Products.FindAsync(productEdit.ProductId);

            if (productFound == null)
            {
                return Constants.RETURNDEFAULTERROR;
            }

            productFound.Name = productEdit.Name;
            productFound.Status = productEdit.Status;
            productFound.Stock = productEdit.Stock;
            productFound.Description = productEdit.Description;
            productFound.Price = productEdit.Price;

            return await _context.SaveChangesAsync();            
        }
    }
}
