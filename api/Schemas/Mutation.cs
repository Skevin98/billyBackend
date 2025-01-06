using api.Event.Services;
using api.Events.Models;
using api.Events.Services;
using api.Tickets.Models;
using api.Tickets.Services;

namespace api.Schemas;

public class Mutation
{
    public async Task<Events.Models.Event> CreateEvent(EventInput input, IEventService eventService)
    {
        var ev = await eventService.Create(input);
        return ev;
    }

    public async Task<Events.Models.Event> UpdateEvent(string eventId, EventInput input, IEventService eventService)
    {
        var ev = await eventService.Update(input, eventId);
        return ev;
    }

    public async Task<TicketType> CreateTicketType(string eventId, TicketTypeInput input,
        ITicketTypeService ticketTypeService)
    {
        var ticketType = await ticketTypeService.Create(eventId, input);
        return ticketType;
    }

    public async Task<TicketType> UpdateTicketType(string eventId, string typeId, TicketTypeInput input,
        ITicketTypeService ticketTypeService)
    {
        var ticketType = await ticketTypeService.Update(eventId, typeId, input);
        return ticketType;
    }
}