using backend.DTOs.Profiles;
using FluentValidation;


namespace backend.Validators.Profiles;

public class ProfileUpdateDtoValidator : AbstractValidator<ProfileUpdateDto>
{
    public ProfileUpdateDtoValidator()
    {
        RuleFor(p => p.FirstName)
            .MinimumLength(2).WithMessage("Минимальная длина текста в поле - 2 символа")
            .MaximumLength(50).WithMessage("Максимальная длина текста в поле - 50 символов")
            .When(c => c.FirstName != null);

        RuleFor(p => p.LastName)
            .MinimumLength(2).WithMessage("Минимальная длина текста в поле - 2 символа")
            .MaximumLength(50).WithMessage("Максимальная длина текста в поле - 50 символов")
            .When(c => c.LastName != null);

        RuleFor(p => p.AboutMe)
            .MinimumLength(3).WithMessage("Минимальная длина текста в поле - 3 символа")
            .MaximumLength(2000).WithMessage("Максимальная длина текста в поле - 2000 символов")
            .When(c => c.AboutMe != null);

        RuleFor(p => p.Profession)
            .MinimumLength(3).WithMessage("Минимальная длина текста в поле - 3 символа")
            .MaximumLength(100).WithMessage("Максимальная длина текста в поле - 100 символов")
            .When(c => c.Profession != null);

        RuleFor(p => p.Birthday)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Дата рождения не может быть в будущем")
            .GreaterThan(new DateOnly(1900, 1, 1)).WithMessage("Дата рождения должна быть позже 1900 года")
            .When(p => p.Birthday.HasValue);
    }
}