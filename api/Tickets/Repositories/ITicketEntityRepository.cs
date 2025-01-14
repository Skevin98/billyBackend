using api.Tickets.Models;

namespace api.Tickets.Repositories;

public interface ITicketEntityRepository
{
    Task<TicketEntity> Create(string eventId, string ticketTypeId, string userId, TicketEntity dummyTicket);
    Task<TicketEntity> Update(string ticketTypeId, string dummyUserId, string ticketId, TicketEntity payload);
}