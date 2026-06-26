using backend.DTOs.PostReactions;
using FluentValidation;


namespace backend.Validators.PostReactions;

public class PostReactionDtoValidator : AbstractValidator<PostReactionDto>
{
    public PostReactionDtoValidator()
    {
        RuleFor(pr => pr.Reaction)
            .IsInEnum().WithMessage("Необходим корректный код реакции");

        RuleFor(pr => pr.PostId)
            .GreaterThan(0).WithMessage("Необходим корректный id");
    }
}