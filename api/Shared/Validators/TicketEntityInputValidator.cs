using api.Tickets.Models;
using FluentValidation;

namespace api.Shared.Validators;

public class TicketEntityInputValidator : AbstractValidator<TicketEntityInput>
{
    public TicketEntityInputValidator()
    {
        RuleFor(input=>input.EventId).NotEmpty().WithMessage("EventId is required");
        RuleFor(input=>input.TicketTypeId).NotEmpty().WithMessage("TicketTypeId is required");
        
    }
}