using api.Tickets.Models;

namespace api.Tickets.Services;

public interface ITicketTypeService
{
    Task<TicketType> Create(string eventId, TicketTypeInput input);
    Task<List<TicketType>> GetAllByEventId(string eventId);
    Task<TicketType?> GetByEventIdAndTicketTypeId(string eventId, string typeId);
    Task<TicketType?> Update(string eventId, string typeId, TicketTypeInput input);
}