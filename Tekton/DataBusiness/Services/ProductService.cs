using BusinessLayer.Common;
using BusinessLayer.Dto;
using BusinessLayer.Interfaces;
using DataLayer;
using FluentValidation;
using LazyCache;
using Microsoft.EntityFrameworkCore;
using static BusinessLayer.Common.EnumerationChange;

namespace BusinessLayer.Services
{
    public class ProductService : IProduct
    {
        readonly ProductsDbContext _context;
        readonly IProductRepository<Product> _productRepository;
        readonly IProductStatusRepository<ProductStatus> _productStatusRepository;
        readonly IAppCache _appCache;
        readonly HttpClient _httpClient;
        private readonly IValidator<ProductChangeDto>? _productValidator;

        public ProductService(ProductsDbContext productsDbContext, IAppCache appCache,
                                IValidator<ProductChangeDto> validatorProduct, HttpClient httpClient, 
                                IProductRepository<Product> repositoryProduct,
                                IProductStatusRepository<ProductStatus> _repositoryProductStatus)
        {
            this._context = productsDbContext;
            this._appCache = appCache;
            this._productValidator = validatorProduct;
            this._httpClient = httpClient;
            this._productRepository = repositoryProduct;
            this._productStatusRepository = _repositoryProductStatus;
        }

        public async Task<ProductDto> AddProduct(ProductChangeDto productInsert)
        {
            try
            {
                var validationResult = await _productValidator.ValidateAsync(productInsert, options => options.IncludeRuleSets(EnumValidationType.Insert.ToString()));

                if (validationResult != null && !validationResult.IsValid)
                {
                    string errores = string.Empty;
                    foreach (var error in validationResult.Errors)
                    {
                        errores += string.Concat(error.ErrorMessage, Environment.NewLine);
                    }

                    return new ProductDto() { IsValid = false, ErrorMessage = errores };
                }

                var idNewProduct = await _productRepository.AddProduct(productInsert);

                return await GetById(idNewProduct);

            }
            catch (Exception ex)
            {
                return new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.EXCEPCION, ex.Message) };
            }
        }

        public async Task<ProductDto> EditProduct(ProductChangeDto productEdit)
        {
            try
            {
                var validationResult = await _productValidator.ValidateAsync(productEdit, options => options.IncludeRuleSets(EnumValidationType.Update.ToString()));

                if (validationResult != null && !validationResult.IsValid)
                {
                    string errores = string.Empty;
                    foreach (var error in validationResult.Errors)
                    {
                        errores += string.Concat(error.ErrorMessage, Environment.NewLine);
                    }

                    return new ProductDto() { IsValid = false, ErrorMessage = errores };
                }

                var productFound = await _productRepository.GetById(productEdit.ProductId);

                if (productFound == null)
                {
                    return new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.PRODUCTNOTFOUND, $"{productEdit.ProductId}") };
                }

                await _productRepository.EditProduct(productEdit);

                return await GetById(productEdit.ProductId);
            }
            catch (Exception ex)
            {
                return new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.EXCEPCION, ex.Message) };
            }
        }

        public async Task<ProductDto> GetById(int idFind)
        {
            try
            {
                var productoDB = await _productRepository.GetById(idFind);

                if (productoDB == null)
                {
                    return new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.PRODUCTNOTFOUND, $"{idFind}") };
                }

                var productFound = new ProductDto
                {
                    ProductId = productoDB.ProductId,
                    Name = productoDB.Name,
                    StatusName = productoDB.Status.ToString(),
                    Stock = productoDB.Stock,
                    Description = productoDB.Description,
                    Price = productoDB.Price
                };

                AssignStatus(productFound);
                GetAndCalculateDiscount(productFound);

                return productFound;
            }
            catch (Exception ex)
            {
                return new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.EXCEPCION, ex.Message) };
            }
        }

        private void GetAndCalculateDiscount(ProductDto productDto)
        {
            productDto.Discount = new ProductDiscountService(this._httpClient).GetDiscountById(productDto.ProductId).Result;
            productDto.FinalPrice = (productDto.Price * (Constants.PERCENT - productDto.Discount) / Constants.PERCENT);
        }

        private void AssignStatus(ProductDto productDto)
        {
            productDto.StatusName = new ProductStatusService(this._context, this._appCache, this._productStatusRepository).GetByValue(Int16.Parse(productDto.StatusName)).Result;
        }
    }
}
