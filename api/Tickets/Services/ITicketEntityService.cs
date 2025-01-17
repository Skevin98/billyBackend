using api.Tickets.Models;

namespace api.Tickets.Services;

public interface ITicketEntityService
{
    Task<TicketEntity> Create( string userId, TicketEntityInput input);
    Task<TicketEntity> Update(string userId, string ticketId, TicketEntityInput input);
}