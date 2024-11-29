using api.Event.Models;
using api.Event.Repositories;
using api.Event.Utils;

namespace test.Mocks;

public class MockEventRepositoryImpl : IEventRepository
{
    private List<Event> events;

    public MockEventRepositoryImpl()
    {
        events = GetEventList();
    }

    public Task<Event?> GetByIdAsync(string i)
    {
        var ev = events.FirstOrDefault(e => e.Id == i);
        return ev == null ? Task.FromResult<Event?>(null) : Task.FromResult(ev);
    }

    public Task<List<Event>> GetByOwnerIdAsync(string owner)
    {
        var ownerEvents = events.FindAll(e => e.OwnerId == owner);
        return Task.FromResult(ownerEvents);
    }

    public Task<Event> CreateAsync(Event payload)
    {
        payload.Id = Guid.NewGuid().ToString();
        events.Add(payload);
        return Task.FromResult(payload);
    }

    public Task<Event?> UpdateAsync(string eventId, Event payload)
    {
        var ev = events.FirstOrDefault(e => e.Id == eventId);
        if (ev == null)
        {
            return Task.FromResult<Event>(null);
        }
        ev = payload;
        ev.Id = eventId.ToString();
        return Task.FromResult(ev);
    }

    private List<Event> GetEventList()
    {
        var events = new List<Event>();
        for (var i = 1; i < 6; i++)
        {
            var ownerId = "owner";
            if (i > 3)
            {
                ownerId = "not-owner";
            }

            var ev = new Event(i.ToString(), ownerId, 
                $"event {i}", $"description of event {i}",
                DateTime.Now, DateTime.Now.AddDays(1), status: EventStatus.SCHEDULED);
            events.Add(ev);
        }
        var updateEv = new Event("6749b8d38f421d74b5787c1e", "new-owner", 
            $"event 63f8e9b8a9a72b4c2fceffff", $"description of event 63f8e9b8a9a72b4c2fceffff",
            DateTime.Now, DateTime.Now.AddDays(1), status: EventStatus.SCHEDULED);
        
        events.Add(updateEv);

        return events;
    }
}