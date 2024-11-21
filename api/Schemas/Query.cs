using api.Event.Models;
using api.Event.Services;

namespace api.Schemas;

public class Query
{

    public async Task<List<Event.Models.Event>> EventsByOwnerId(string ownerId, IEventService eventService)
    {
        var events = await eventService.GetByOwnerId(ownerId);
        return events;
    }

    public async Task<Event.Models.Event?> EventById(string id, IEventService eventService)
    {
        var ev = await eventService.GetById(id);
        return ev;
    }
    
}