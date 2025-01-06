namespace api.Events.Repositories;

public interface IEventRepository
{
    Task<Events.Models.Event?> GetByIdAsync(string i);
    Task<List<Events.Models.Event>> GetByOwnerIdAsync(string owner);
    Task<Events.Models.Event> CreateAsync(Events.Models.Event payload);
    Task<Events.Models.Event?> UpdateAsync(string eventId, Events.Models.Event payload);
}