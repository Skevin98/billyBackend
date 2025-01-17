using api.Tickets.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Tickets.Models;

public class TicketEntityInput
{
    [ID]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string EventId { get; set; }
    
    public string TicketTypeId { get; set; }
    
    public string Order { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
    
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    [BsonRepresentation(BsonType.String)]
    public TicketStatus? Status { get; set; } = TicketStatus.CREATED;
}