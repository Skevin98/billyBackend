using api.Tickets.Models;
using FluentValidation;

namespace api.Shared.Validators;

public class TicketTypeInputValidator : AbstractValidator<TicketTypeInput>
{
    public TicketTypeInputValidator()
    {
        RuleFor(input => input.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(input => input.Price).NotEmpty().WithMessage("Price is required");
    }
}