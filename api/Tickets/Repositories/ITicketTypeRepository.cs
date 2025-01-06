using api.Tickets.Models;

namespace api.Tickets.Repositories;

public interface ITicketTypeRepository
{
    Task<TicketType> CreateAsync(string eventId, TicketType ticketType);
    Task<List<TicketType>> GetAllByEventId(string eventId);
    Task<TicketType?> GetByEventIdAndTicketTypeId(string eventId, string typeId);
    Task<TicketType?> UpdateAsync(string eventId, string typeId, TicketType payload);
}