

using api.Events.Models;
using api.Events.Services;
using api.Tickets.Models;
using api.Tickets.Services;

namespace api.Schemas;

public class Query
{

    public async Task<List<Events.Models.Event>> EventsByOwnerId(string ownerId, IEventService eventService)
    {
        var events = await eventService.GetByOwnerId(ownerId);
        return events;
    }

    public async Task<Events.Models.Event?> EventById(string id, IEventService eventService)
    {
        var ev = await eventService.GetById(id);
        return ev;
    }

    public async Task<List<TicketType>> TicketsByEventId(string eventId, ITicketTypeService ticketService)
    {
        var type = await ticketService.GetAllByEventId(eventId);
        return type;
    }
    
    public async Task<TicketType?> TicketsByEventIdAndTicketId(string eventId, string typeId,
        ITicketTypeService ticketService)
    {
        var type = await ticketService.GetByEventIdAndTicketTypeId(eventId, typeId);
        return type;
    }
    
}