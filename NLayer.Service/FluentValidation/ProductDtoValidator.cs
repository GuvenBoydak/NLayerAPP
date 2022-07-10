using FluentValidation;
using NLayer.Core;

namespace NLayer.Service
{
    public class ProductDtoValidator:AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is Required").NotEmpty().WithMessage("{PropertyName} is Required");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("{PropertyName} is greater 0").NotEmpty().WithMessage("{PropertyName} is Required");

            RuleFor(x => x.Stock).InclusiveBetween(1,int.MaxValue).WithMessage("{PropertyName} is greater 0").NotEmpty().WithMessage("{PropertyName} is Required");
        }
    }
}
