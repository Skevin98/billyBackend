using api.Event.Utils;
using api.Events.Models;
using api.Tickets.Models;
using api.Tickets.Repositories;
using api.Tickets.Utils;
using api.Users.Models;

namespace test.Mocks;

public class MockTicketEntityRepositoryImpl : ITicketEntityRepository
{
    private Event dummyEvent;
    private string ownerId = "new-owner";
    private string eventId = "63f8ebd7e6b27d1f45ef05b5";
    private string name = "event1";
    private string description = "description";
    private string location = "location";
    private string ticketTypeId = "677be50cd5238983a28e8a6b";
    private DateTime startDate = DateTime.Now.AddDays(3);
    private DateTime endDate = DateTime.Now.AddDays(6);
    private UserEntity dummyUser;
    private string dummyUserId = new Guid().ToString();

    public MockTicketEntityRepositoryImpl()
    {
        var dummyType = new TicketType
        {
            Id = ticketTypeId ,
            Description = "description",
            Title = "title",
            Price = 100
        };
        dummyEvent = new Event(eventId, ownerId,
            name, description,
            startDate, endDate, lastModifiedDate: DateTime.Now, status: EventStatus.SCHEDULED);
        
        dummyEvent.TicketTypes.Add(dummyType);
        
        dummyUser = new UserEntity
        {
            Id = dummyUserId,
        };
        
        dummyUser.TicketsPurchased.Add(new TicketEntity
        {
            Id = "xxx",
            Status = TicketStatus.SOLD,
            EventId = eventId,
            TicketTypeId =ticketTypeId,
            Order = "billy-1"
        });
    }
    public Task<TicketEntity> Create(string eventId, string ticketTypeId,string userId, TicketEntity dummyTicket)
    {
        if (this.ticketTypeId != ticketTypeId)
        {
            throw new Exception("Ticket type not found");
        }
        if (this.eventId != eventId)
        {
            throw new Exception("Event not found");
        }
        dummyTicket.Id = new Guid().ToString();
        dummyTicket.Order = "billy-"+dummyUser.TicketsPurchased.Count + 1;
        dummyUser.TicketsPurchased.Add(dummyTicket);
        return Task.FromResult(dummyTicket);
    }

    public Task<TicketEntity> Update(string ticketTypeId, string dummyUserId, string ticketId,TicketEntity payload)
    {
        var ticketIndexToUpdate = dummyUser.TicketsPurchased.FindIndex(t => t.Id == ticketId);
        if (ticketIndexToUpdate == -1)
        {
            throw new Exception("Ticket not found");
        }
        dummyUser.TicketsPurchased[ticketIndexToUpdate] = payload;
        return Task.FromResult(dummyUser.TicketsPurchased[ticketIndexToUpdate]);
    }
}