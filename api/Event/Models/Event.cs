using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Event.Models;

public class Event
{
    public Event(string? id, string? ownerId, string name, string? description, 
        DateTime? startDate, DateTime? endDate, DateTime? createdDate = null, DateTime? lastModifiedDate = null)
    {
        Id = id;
        OwnerId = ownerId;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        Name = name;
        CreatedDate = createdDate ?? DateTime.UtcNow;
        LastModifiedDate = lastModifiedDate;
    }

    public Event(EventInput eventInput)
    {
        Id = eventInput.Id;
        OwnerId = eventInput.OwnerId;
        Name = eventInput.Name;
        Description = eventInput.Description;
        StartDate = eventInput.StartDate;
        EndDate = eventInput.EndDate;
        CreatedDate = eventInput.CreatedDate ?? DateTime.UtcNow;
        LastModifiedDate = eventInput.LastModifiedDate ?? null;
    }

    [ID]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? OwnerId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}