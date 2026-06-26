using backend.DTOs.Topics;
using FluentValidation;


namespace backend.Validators.Topics;

public class TopicUpdateDtoValidator : AbstractValidator<TopicUpdateDto>
{
    public TopicUpdateDtoValidator()
    {
        RuleFor(t => t.Title)
            .MinimumLength(4).WithMessage("Минимальная длина текста в поле - 4 символа")
            .MaximumLength(150).WithMessage("Максимальная длина текста в поле - 150 символов")
            .When(t => t.Title != null);

        RuleFor(t => t.Description)
            .MinimumLength(10).WithMessage("Минимальная длина текста в поле - 10 символа")
            .MaximumLength(2000).WithMessage("Максимальная длина текста в поле - 2000 символов")
            .When(t => t.Description != null);


    }
}