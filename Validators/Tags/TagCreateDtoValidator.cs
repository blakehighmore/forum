using backend.DTOs.Tags;
using FluentValidation;


namespace backend.Validators.Tags;

public class TagCreateDtoValidator : AbstractValidator<TagCreateDto>
{
    public TagCreateDtoValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("Поле Title обязательно для заполнения")
            .MinimumLength(1).WithMessage("Минимальная длина текста в поле - 1 символа")
            .MaximumLength(20).WithMessage("Максимальная длина текста в поле - 20 символов");

        RuleFor(t => t.Color)
            .NotEmpty().WithMessage("Поле Color обязательно для заполнения")
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage("Необходим корректный Hex");
    }
}