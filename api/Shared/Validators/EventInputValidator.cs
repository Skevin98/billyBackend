using api.Events.Models;
using FluentValidation;

namespace api.Shared.Validators;

public class EventInputValidator : AbstractValidator<EventInput>
{
    public EventInputValidator()
    {
        RuleFor(eventInput => eventInput.Name).NotEmpty();
        RuleFor(eventInput => eventInput.OwnerId).NotEmpty();
    }
}