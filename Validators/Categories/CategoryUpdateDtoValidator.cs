using backend.DTOs.Categories;
using FluentValidation;


namespace backend.Validators.Categories;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(c => c.Title)
            .MinimumLength(4).WithMessage("Минимальная длина текста в поле - 4 символа")
            .MaximumLength(30).WithMessage("Максимальная длина текста в поле - 30 символов")
            .When(c => c.Title != null);

        RuleFor(c => c.Description)
            .MinimumLength(10).WithMessage("Минимальная длина текста в поле - 10 символа")
            .MaximumLength(2000).WithMessage("Максимальная длина текста в поле - 2000 символов")
            .When(c => c.Description != null);
    }
}