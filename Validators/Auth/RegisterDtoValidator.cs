using backend.DTOs.Auth;
using FluentValidation;


namespace backend.Validators.Auth;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty().WithMessage("Username - обязательное поле")
            .MinimumLength(3).WithMessage("Минимальная длина - 3 символа")
            .MaximumLength(20).WithMessage("Максимальная длина - 20 символов");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Пароль - обязательное поле")
            .MinimumLength(8).WithMessage("Минимальная длина - 8 символа")
            .MaximumLength(72).WithMessage("Максимальная длина - 72 символов");
    }
}