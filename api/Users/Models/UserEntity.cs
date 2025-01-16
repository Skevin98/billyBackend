using api.Tickets.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Users.Models;

public class UserEntity
{
    [ID]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [UseFiltering]
    public List<TicketEntity> TicketsPurchased { get; set; } = [];
    
    public DateTime? LastModifiedDate { get; set; }
    
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
}