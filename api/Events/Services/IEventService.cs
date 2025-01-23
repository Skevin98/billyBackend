using api.Events.Models;

namespace api.Events.Services;

public interface IEventService
{
    Task<Events.Models.Event?> GetById(string eventId);
    Task<List<Events.Models.Event>> GetByOwnerId(string ownerId);
    Task<List<Events.Models.Event>> GetAll();
    Task<Events.Models.Event> Create(EventInput eventInput);
    Task<Events.Models.Event> Update(EventInput eventInput, string eventId);
}