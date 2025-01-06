using api.Shared.Validators;
using api.Tickets.Models;
using api.Tickets.Repositories;
using FluentValidation;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace api.Tickets.Services.Impl;

public class TicketTypeServiceImpl : ITicketTypeService
{
    private readonly ITicketTypeRepository _ticketTypeRepository;
    public TicketTypeServiceImpl(ITicketTypeRepository ticketTypeRepository)
    {
        _ticketTypeRepository = ticketTypeRepository;
    }


    public async Task<TicketType> Create(string eventId, TicketTypeInput input)
    {
        try
        {
            var validator = new TicketTypeInputValidator();
            await validator.ValidateAndThrowAsync(input);
            var ticketTypeToCreate = new TicketType(input);
            var ticket = await _ticketTypeRepository.CreateAsync(eventId, ticketTypeToCreate);
            return ticket;
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<TicketType>> GetAllByEventId(string eventId)
    {
        var tickets = await _ticketTypeRepository.GetAllByEventId(eventId);
        return tickets;
    }

    public async Task<TicketType?> GetByEventIdAndTicketTypeId(string eventId, string typeId)
    {
        var ticket = await _ticketTypeRepository.GetByEventIdAndTicketTypeId(eventId,typeId);
        return ticket;
    }

    public async Task<TicketType?> Update(string eventId, string typeId, TicketTypeInput input)
    {
        try
        {
            var typeToUpdate = await _ticketTypeRepository.GetByEventIdAndTicketTypeId(eventId, input.Id);
            if (typeToUpdate == null)
                throw new KeyNotFoundException($"Ticket type with Id {eventId} not found.");
            var validator = new TicketTypeInputValidator();
            await validator.ValidateAndThrowAsync(input);
            var ticketTypeToCreate = new TicketType(input);
            var ticket = await _ticketTypeRepository.UpdateAsync(eventId, typeId, ticketTypeToCreate);
            return ticket;
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}