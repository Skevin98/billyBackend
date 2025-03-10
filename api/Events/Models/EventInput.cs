using api.Event.Utils;

namespace api.Events.Models;

public class EventInput
{

    public EventInput(string? ownerId, DateTime? startDate, DateTime? endDate, string name,
        EventStatus eventStatus,
        string? description = null,
        DateTime? lastModifiedDate = null, DateTime? createdDate = null, string? id = null)
    {
        Id = id;
        OwnerId = ownerId;
        StartDate = startDate;
        EndDate = endDate;
        Name = name;
        Description = description;
        LastModifiedDate = lastModifiedDate;
        CreatedDate = createdDate;
        EventStatus = eventStatus;
    }

    public string? Id { get; set; }
    public string? OwnerId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public EventStatus EventStatus { get; set; }
}