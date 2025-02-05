using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Tickets.Models;

public class TicketType
{

    public TicketType(TicketTypeInput input)
    {
        Id = input.Id;
        Price = input.Price;
        Title = input.Title;
        Description = input.Description;
        CreatedDate = input.CreatedDate ?? DateTime.UtcNow;
        LastModifiedDate = input.LastModifiedDate ?? null;
    }

    public TicketType() { }

    [GraphQLType(typeof(IdType))]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public double Price { get; set; }

    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
    
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public int TicketsSold { get; set; } = 0;
}