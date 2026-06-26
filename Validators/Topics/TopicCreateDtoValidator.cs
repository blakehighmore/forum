using backend.DTOs.Topics;
using FluentValidation;


namespace backend.Validators.Topics;

public class TopicCreateDtoValidator : AbstractValidator<TopicCreateDto>
{
    public TopicCreateDtoValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("Поле Title обязательно для заполнения")
            .MinimumLength(4).WithMessage("Минимальная длина текста в поле - 4 символа")
            .MaximumLength(150).WithMessage("Максимальная длина текста в поле - 150 символов");

        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Поле Description обязательно для заполнения")
            .MinimumLength(10).WithMessage("Минимальная длина текста в поле - 10 символа")
            .MaximumLength(2000).WithMessage("Максимальная длина текста в поле - 2000 символов");

        RuleForEach(t => t.TagIds).GreaterThan(0).WithMessage("Введите корректные id");

        RuleFor(t => t.CategoryId)
            .GreaterThan(0).WithMessage("Идентификатор категории должен быть корректным");
    }
}