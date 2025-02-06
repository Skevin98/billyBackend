using api.Tickets.Models;
using api.Users.Models;

namespace api.Tickets.Services;

public interface ITicketEntityService
{
    Task<TicketEntity> Create( string userId, TicketEntityInput input);
    Task<TicketEntity> Update(string userId, string ticketId, TicketEntityInput input);
    public Task<List<UserEntity>> getUsersTicketsByEventId(string eventId);
}