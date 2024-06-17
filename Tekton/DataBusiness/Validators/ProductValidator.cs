using BusinessLayer.Dto;
using FluentValidation;
using static BusinessLayer.Common.EnumerationChange;

namespace BusinessLayer.Validators
{
    public  class ProductValidator : AbstractValidator<ProductChangeDto>
    {
        public string errorMessage { get; set; } = string.Empty;

        public ProductValidator() 
        {
            RuleSet(EnumValidationType.Insert.ToString(), () =>
            {
                errorMessage = "Error al insertar producto: ";
                RuleFor(item => item.Name).MinimumLength(5).WithMessage(string.Concat(errorMessage, "El nombre del producto debe tener mínimo 5 letras"));
                RuleFor(item => item.Status).InclusiveBetween(0, 1).WithMessage(string.Concat(errorMessage, "El status del producto debe ser 1 o 0"));
                RuleFor(item => item.Stock).GreaterThan(-1).WithMessage(string.Concat(errorMessage, "La cantidad en stock del producto debe ser mayor que -1"));
                RuleFor(item => item.Description).MinimumLength(5).WithMessage(string.Concat(errorMessage, "La descripción del producto debe tener mínimo 5 letras"));
                RuleFor(item => item.Price).GreaterThan(0).WithMessage(string.Concat(errorMessage, "Debe indicar un precio de producto mayor que cero"));
            });

            RuleSet(EnumValidationType.Update.ToString(), () =>
            {
                errorMessage = "Error al actualizar producto: ";                
                RuleFor(item => item.Name).MinimumLength(5).WithMessage(string.Concat(errorMessage, "El nombre del producto debe tener mínimo 5 letras"));
                RuleFor(item => item.Status).InclusiveBetween(0, 1).WithMessage(string.Concat(errorMessage, "El status del producto debe ser 1 o 0"));
                RuleFor(item => item.Stock).GreaterThan(-1).WithMessage(string.Concat(errorMessage, "La cantidad en stock del producto debe ser mayor que -1"));
                RuleFor(item => item.Description).MinimumLength(5).WithMessage(string.Concat(errorMessage, "La descripción del producto debe tener mínimo 5 letras"));
                RuleFor(item => item.Price).GreaterThan(0).WithMessage(string.Concat(errorMessage, "Debe indicar un precio de producto mayor que cero"));
            });
        }
    }
}
