using BusinessLayer.Common;
using BusinessLayer.Dto;
using BusinessLayer.Filters;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Tekton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private const int DEFAULTCODE = 1;
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
            _product = product;
        }

        [ServiceFilter(typeof(ResponseTimeFilter))]        
        [HttpGet("GetById/{idFind}")]
        public async Task<ActionResult<ProductDto>> GetById(int idFind) 
        {
            var result = await _product.GetById(idFind);
                        
            if (!result.IsValid && result.ErrorMessage.Contains(Constants.EXCEPCION))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);                
            }

            if (!result.IsValid && !result.ErrorMessage.Contains(Constants.EXCEPCION))
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result);
        }

        [ServiceFilter(typeof(ResponseTimeFilter))]
        [HttpPost("Insert")]
        public async Task<ActionResult<ProductDto>> InsertProduct(ProductChangeDto product)
        {
            var result = await _product.AddProduct(product);

            if (!result.IsValid && result.ErrorMessage.Contains(Constants.EXCEPCION))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
            }

            if (!result.IsValid && !result.ErrorMessage.Contains(Constants.EXCEPCION))
            {
                return BadRequest(result.ErrorMessage);
            }
            
            return CreatedAtAction(nameof(GetById), new { idFind = result is null ? DEFAULTCODE : result.ProductId }, result);
        }

        [ServiceFilter(typeof(ResponseTimeFilter))]
        [HttpPut("Update")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(ProductChangeDto product)
        {
            var result = await _product.EditProduct(product);

            if (!result.IsValid && result.ErrorMessage.Contains(Constants.EXCEPCION))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
            }

            if (!result.IsValid && !result.ErrorMessage.Contains(Constants.EXCEPCION))
            {
                return BadRequest(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetById), new { idFind = result is null ? DEFAULTCODE : result.ProductId }, result);
        }
    }
}
