namespace api.Event.Services;
using Models;

public interface IEventService
{
    Task<Event?> GetById(string eventId);
    Task<List<Event>> GetByOwnerId(string ownerId);
    Task<Event> Create(EventInput eventInput);
    Task<Event> Update(EventInput eventInput, string eventId);
}