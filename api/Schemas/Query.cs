

using api.Events.Models;
using api.Events.Services;
using api.Tickets.Models;
using api.Tickets.Services;
using api.Users.Models;
using api.Users.Services;

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

    [UseFiltering]
    public async Task<List<Events.Models.Event>> AllEvents(IEventService eventService)
    {
        var events = await eventService.GetAll();
        return events;
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
    
    public async Task<UserEntity?> UserById(string id, IUserService userService)
    {
        var user = await userService.getUserById(id);
        return user;
    }

    public async Task<List<UserEntity>> GetUsersTicketsByEventId(string eventId, ITicketEntityService ticketService)
    {
        var res = await ticketService.getUsersTicketsByEventId(eventId);
        return res;
    }
    
}