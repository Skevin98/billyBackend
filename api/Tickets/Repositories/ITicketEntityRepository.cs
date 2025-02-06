using api.Tickets.Models;

namespace api.Tickets.Repositories;

public interface ITicketEntityRepository
{
    Task<TicketEntity> Create( string userId, TicketEntity dummyTicket);
    Task<TicketEntity> Update(string userId, string ticketId, TicketEntity payload);
}