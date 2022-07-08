using FluentValidation;
using NLayer.Core;

namespace NLayer.Service
{
    public class CategoryDtoValidator:AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is Required").NotNull().WithMessage("{PropertyName} is Required");
        }
    }
}
