using backend.DTOs.Posts;
using FluentValidation;


namespace backend.Validators.Posts;

public class PostCreateDtoValidator : AbstractValidator<PostCreateDto>
{
    public PostCreateDtoValidator()
    {
        RuleFor(p => p.Content)
            .NotEmpty().WithMessage("Поле Content обязательно для заполнения")
            .MinimumLength(10).WithMessage("Минимальная длина текста в поле - 10 символа")
            .MaximumLength(2000).WithMessage("Максимальная длина текста в поле - 2000 символов");

        RuleFor(t => t.TopicId)
            .GreaterThan(0).WithMessage("Идентификатор темы должен быть корректным");
    }
}