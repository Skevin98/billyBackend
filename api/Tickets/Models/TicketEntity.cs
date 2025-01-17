using api.Tickets.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Tickets.Models;


public class TicketEntity
{
    public static TicketEntity FromInput(TicketEntityInput input)
    {
        var entity = new TicketEntity
        {
            Id = input.Id,
            EventId = input.EventId,
            TicketTypeId = input.TicketTypeId,
            Order = input.Order,
            Status = input.Status ?? TicketStatus.CREATED,
            CreatedDate = input.CreatedDate ?? DateTime.UtcNow,
            LastModifiedDate = input.LastModifiedDate ?? null
        };
         return entity;
    }
    
    [ID]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public string EventId { get; set; }
    
    public string TicketTypeId { get; set; }
    
    public string Order { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
    
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    [BsonRepresentation(BsonType.String)]
    public TicketStatus Status { get; set; } = TicketStatus.CREATED;
}