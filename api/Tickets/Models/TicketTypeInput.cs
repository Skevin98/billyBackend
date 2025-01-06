using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Tickets.Models;

public class TicketTypeInput
{
    [ID]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public double Price { get; set; } = 1.0;

    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
    
    public DateTime? CreatedDate { get; set; }
}