using api.Event.Models;
using api.Event.Repositories;
using api.Event.Services;

namespace api.Schemas;

public class Mutation
{
    
        public async Task<Event.Models.Event> CreateEvent(EventInput input, IEventService eventService)
        {
            var ev = await eventService.Create(input);
            return ev;
        }

        public async Task<Event.Models.Event> UpdateEvent(string eventId, EventInput input, IEventService eventService)
        {
            var ev = await eventService.Update(input, eventId);
            return ev;
        }
}