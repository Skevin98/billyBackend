namespace api.Event.Repositories;
using Models;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(string i);
    Task<List<Event>> GetByOwnerIdAsync(string owner);
    Task<Event> CreateAsync(Event payload);
    Task<Event?> UpdateAsync(string eventId, Event payload);
}