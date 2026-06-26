using backend.DTOs.Categories;
using FluentValidation;


namespace backend.Validators.Categories;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{

    public CategoryCreateDtoValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Поле Title обязательно для заполнения")
            .MinimumLength(4).WithMessage("Минимальная длина текста в поле - 4 символа")
            .MaximumLength(30).WithMessage("Максимальная длина текста в поле - 30 символов");
        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Поле Description обязательно для заполнения")
            .MinimumLength(10).WithMessage("Минимальная длина текста в поле - 10 символа")
            .MaximumLength(2000).WithMessage("Максимальная длина текста в поле - 2000 символов");
    }
}