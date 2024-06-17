using BusinessLayer.Common;
using BusinessLayer.Dto;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tekton.Controllers.Tests
{
    [TestClass()]
    public class ProductControllerTests
    {
        private readonly Mock<IProduct> productMock;
        private readonly ProductController productController;
        
        public ProductControllerTests()
        {
            productMock = new Mock<IProduct>();
            productController = new ProductController(productMock.Object);
        }

        [TestMethod()]
        public void GetById_IdNotExists_Return_NotFound_Test()
        {
            // Arrange
            int productId = 99;
            var objectToReturn = new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.PRODUCTNOTFOUND, $"{productId}") };
            productMock.Setup(service => service.GetById(productId)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.GetById(productId);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            Xunit.Assert.IsType<NotFoundObjectResult>(actionResult.Result.Result);
        }

        [TestMethod()]
        public void GetById_GenerateException_Return_Status500_Test()
        {
            // Arrange
            int productId = 3;
            var objectToReturn = new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.EXCEPCION, "Error in server") };
            productMock.Setup(service => service.GetById(productId)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.GetById(productId);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            var result = (Microsoft.AspNetCore.Mvc.ObjectResult?)actionResult.Result.Result;
            Xunit.Assert.Equal(500, result?.StatusCode);
        }

        [TestMethod()]
        public void GetById_FindIdSuccessfully_Return_Ok_Test()
        {
            // Arrange
            int productId = 6;
            var objectToReturn = new ProductDto() 
            { 
                ProductId = 6,
                Name = "Teclado",
                StatusName = "Active",
                Stock = 5,
                Description = "Teclado inhalámbrico",
                Price = 14600,
                Discount = 25,
                FinalPrice = 10950,
                IsValid = true,
                ErrorMessage = ""
            };
            productMock.Setup(service => service.GetById(productId)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.GetById(productId);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            Xunit.Assert.IsType<OkObjectResult>(actionResult.Result.Result);            
            var result = (ProductDto?)((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult.Result.Result).Value;
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(6, result?.ProductId);
        }

        [TestMethod()]
        public void InsertProduct_NameWithOnlyThreeLetters_Return_BadRequest_Test()
        {
            // Arrange
            var productInsert = new ProductChangeDto()
            { 
                Name = "Nue",
                Status = 1,
                Stock = 3,
                Description = "Descripción nuevo producto",
                Price = 12300
            };
            var objectToReturn = new ProductDto() { IsValid = false, ErrorMessage = "Error al insertar producto: El nombre del producto debe tener mínimo 5 letras\r\n" };
            productMock.Setup(service => service.AddProduct(productInsert)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.InsertProduct(productInsert);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            Xunit.Assert.IsType<BadRequestObjectResult>(actionResult.Result.Result);
            var result = ((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult.Result.Result).Value;
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(objectToReturn.ErrorMessage, result);
        }

        [TestMethod()]
        public void InsertProduct_PriceMinorThanZero_Return_BadRequest_Test()
        {
            // Arrange
            var productInsert = new ProductChangeDto()
            {
                Name = "Nuevo Producto",
                Status = 1,
                Stock = 3,
                Description = "Descripción nuevo producto",
                Price = -1
            };
            var objectToReturn = new ProductDto() { IsValid = false, ErrorMessage = "Error al insertar producto: Debe indicar un precio de producto mayor que cero\r\n" };
            productMock.Setup(service => service.AddProduct(productInsert)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.InsertProduct(productInsert);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            Xunit.Assert.IsType<BadRequestObjectResult>(actionResult.Result.Result);
            var result = ((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult.Result.Result).Value;
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(objectToReturn.ErrorMessage, result);
        }

        [TestMethod()]
        public void InsertProduct_GenerateException_Return_Status500_Test()
        {
            // Arrange
            var productInsert = new ProductChangeDto()
            {
                Name = "Nuevo Producto",
                Status = 1,
                Stock = 3,
                Description = "Descripción nuevo producto",
                Price = -1
            };
            var objectToReturn = new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.EXCEPCION, "Error in database") };
            productMock.Setup(service => service.AddProduct(productInsert)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.InsertProduct(productInsert);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            var result = (Microsoft.AspNetCore.Mvc.ObjectResult?)actionResult.Result.Result;
            Xunit.Assert.Equal(500, result?.StatusCode);
            var value = result?.Value;
            Xunit.Assert.NotNull(value);
            Xunit.Assert.Equal(objectToReturn.ErrorMessage, value);
        }

        [TestMethod()]
        public void InsertProduct_InsertSuccessfully_Return_CreatedAtAction_Test()
        {
            // Arrange
            var productInsert = new ProductChangeDto()
            {
                Name = "Nuevo Producto",
                Status = 1,
                Stock = 3,
                Description = "Descripción nuevo producto",
                Price = 25300
            };
            var objectToReturn = new ProductDto() 
            {
                ProductId = 11,
                Name = "Nuevo Producto",
                StatusName = "Active",
                Stock = 3,
                Description = "Descripción nuevo producto",
                Price = 25300,
                Discount = 50,
                IsValid = true,
                ErrorMessage = ""
            };
            productMock.Setup(service => service.AddProduct(productInsert)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.InsertProduct(productInsert);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            Xunit.Assert.IsType<CreatedAtActionResult>(actionResult.Result.Result);
            var result = (ProductDto?)((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult.Result.Result).Value;
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(11, result?.ProductId);
            Xunit.Assert.Equal("Active", result?.StatusName);
        }

        [TestMethod()]
        public void UpdateProduct_DescriptionWithOnlyFourLetters_Return_BadRequest_Test()
        {
            // Arrange
            var productUpdate = new ProductChangeDto()
            {
                Name = "Nuevo producto",
                Status = 1,
                Stock = 3,
                Description = "Desc",
                Price = 12300
            };
            var objectToReturn = new ProductDto() { IsValid = false, ErrorMessage = "Error al actualizar producto: La descripción del producto debe tener mínimo 5 letras\r\n" };
            productMock.Setup(service => service.EditProduct(productUpdate)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.UpdateProduct(productUpdate);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            Xunit.Assert.IsType<BadRequestObjectResult>(actionResult.Result.Result);
            var result = ((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult.Result.Result).Value;
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(objectToReturn.ErrorMessage, result);
        }

        [TestMethod()]
        public void UpdateProduct_StockMinorThanZero_Return_BadRequest_Test()
        {
            // Arrange
            var productUpdate = new ProductChangeDto()
            {
                Name = "Nuevo Producto",
                Status = 1,
                Stock = -1,
                Description = "Descripción nuevo producto",
                Price = 12650
            };
            var objectToReturn = new ProductDto() { IsValid = false, ErrorMessage = "Error al actualizar producto: La cantidad en stock del producto debe ser mayor que -1\r\n" };
            productMock.Setup(service => service.EditProduct(productUpdate)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.UpdateProduct(productUpdate);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            Xunit.Assert.IsType<BadRequestObjectResult>(actionResult.Result.Result);
            var result = ((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult.Result.Result).Value;
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(objectToReturn.ErrorMessage, result);
        }

        [TestMethod()]
        public void UpdateProduct_GenerateException_Return_Status500_Test()
        {
            // Arrange
            var productUpdate = new ProductChangeDto()
            {
                Name = "Nuevo Producto",
                Status = 1,
                Stock = 3,
                Description = "Descripción nuevo producto",
                Price = -1
            };
            var objectToReturn = new ProductDto() { IsValid = false, ErrorMessage = string.Concat(Constants.EXCEPCION, "Error in networking") };
            productMock.Setup(service => service.EditProduct(productUpdate)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.UpdateProduct(productUpdate);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            var result = (Microsoft.AspNetCore.Mvc.ObjectResult?)actionResult.Result.Result;
            Xunit.Assert.Equal(500, result?.StatusCode);
            var value = result?.Value;
            Xunit.Assert.NotNull(value);
            Xunit.Assert.Equal(objectToReturn.ErrorMessage, value);
        }

        [TestMethod()]
        public void UpdateProduct_UpdateSuccessfully_Return_CreatedAtAction_Test()
        {
            // Arrange
            var productUpdate = new ProductChangeDto()
            {
                Name = "Nuevo Nombre",
                Status = 1,
                Stock = 3,
                Description = "Descripción nuevo nombre",
                Price = 18420
            };
            var objectToReturn = new ProductDto()
            {
                ProductId = 2,
                Name = "Nuevo Nombre",
                StatusName = "Active",
                Stock = 3,
                Description = "Descripción nuevo nombre",
                Price = 18420,
                Discount = 10,
                IsValid = true,
                ErrorMessage = ""
            };
            productMock.Setup(service => service.EditProduct(productUpdate)).ReturnsAsync(objectToReturn);

            // Act
            var actionResult = productController.UpdateProduct(productUpdate);

            // Assert
            Xunit.Assert.NotNull(actionResult);
            Xunit.Assert.IsType<CreatedAtActionResult>(actionResult.Result.Result);
            var result = (ProductDto?)((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult.Result.Result).Value;
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result?.ProductId);
            Xunit.Assert.Equal(18420, result?.Price);
        }

    }
}