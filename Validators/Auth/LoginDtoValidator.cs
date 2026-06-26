using backend.DTOs.Auth;
using FluentValidation;


namespace backend.Validators.Auth;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty().WithMessage("Username - обязательное поле");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Пароль - обязательное поле")
            .MinimumLength(8).WithMessage("Минимальная длина - 8 символа")
            .MaximumLength(72).WithMessage("Максимальная длина - 72 символов");
    }
}