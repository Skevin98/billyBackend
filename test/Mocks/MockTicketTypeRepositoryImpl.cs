using api.Event.Utils;
using api.Events.Models;
using api.Tickets.Models;
using api.Tickets.Repositories;

namespace test.Mocks;

public class MockTicketTypeRepositoryImpl : ITicketTypeRepository
{
    
    private Event dummyEvent;
    private string ownerId = "new-owner";
    private string eventId = "6749b8d38f421d74b5787c1e";
    private string name = "event1";
    private string description = "description";
    private string location = "location";
    private string eventType = "eventType";
    private DateTime startDate = DateTime.Now.AddDays(3);
    private DateTime endDate = DateTime.Now.AddDays(6);
    
    public MockTicketTypeRepositoryImpl()
    {
        var dummyType = new TicketType
        {
            Id = "675b4477f96cb61f898ce956" ,
            Description = "description",
            Title = "title",
            Price = 100
        };
        dummyEvent = new Event(eventId, ownerId,
            name, description,
            startDate, endDate, lastModifiedDate: DateTime.Now, status: EventStatus.SCHEDULED);
        
        dummyEvent.TicketTypes.Add(dummyType);
    }

    public Task<TicketType> CreateAsync(string eventId, TicketType ticketType)
    {
        if (eventId != dummyEvent.Id)
            throw new Exception("Event not found");
        ticketType.Id = Guid.NewGuid().ToString();
        
        dummyEvent.TicketTypes.Add(ticketType);
        return Task.FromResult(ticketType);
    }

    public Task<List<TicketType>> GetAllByEventId(string eventId)
    {
        List<TicketType> ticketTypes = new List<TicketType>();
        for (int i = 1; i < 4; i++)
        {
            var ticketType = new TicketType
            {
                Id = Guid.NewGuid().ToString(),
                Title = $"Title {i}",
                Description = $"Description {i}",
                Price = 100 * i
            };
            ticketTypes.Add(ticketType);
        }
        
        return Task.FromResult(ticketTypes);
    }

    public Task<TicketType?> GetByEventIdAndTicketTypeId(string eventId, string typeId)
    {
        if (eventId != dummyEvent.Id)
            return Task.FromResult<TicketType?>(null);
        var type = dummyEvent.TicketTypes.Find(x=> x.Id == typeId);
        if (type is not null)
        {
            return Task.FromResult(type);
        }
        throw new KeyNotFoundException($"No ticket type found for event {eventId}");
    }

    public Task<TicketType> UpdateAsync(string eventId, string typeId, TicketType payload)
    {
        if (eventId != dummyEvent.Id)
            throw new Exception("Event not found");
        var type = dummyEvent.TicketTypes.Find(x=> x.Id == typeId);
        if (type is not null)
        {
            payload.Id = type.Id;
            payload.LastModifiedDate = DateTime.Now;
            payload.CreatedDate = DateTime.Now.AddDays(-2);
            return Task.FromResult(payload);
        }
        throw new KeyNotFoundException($"No ticket type found for event {eventId}");
    }
}