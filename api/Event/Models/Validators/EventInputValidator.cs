using FluentValidation;

namespace api.Event.Models.Validators;

public class EventInputValidator : AbstractValidator<EventInput>
{
    public EventInputValidator()
    {
        RuleFor(eventInput => eventInput.Name).NotEmpty();
        RuleFor(eventInput => eventInput.OwnerId).NotEmpty();
    }
}