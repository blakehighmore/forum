using backend.DTOs.Posts;
using FluentValidation;


namespace backend.Validators.Posts;

public class PostUpdateDtoValidator : AbstractValidator<PostUpdateDto>
{
    public PostUpdateDtoValidator()
    {
        RuleFor(p => p.Content)
            .MinimumLength(10).WithMessage("Минимальная длина текста в поле - 10 символа")
            .MaximumLength(2000).WithMessage("Максимальная длина текста в поле - 2000 символов")
            .When(c => c.Content != null);
    }
}