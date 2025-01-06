using api.Shared;
using api.Tickets.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Tickets.Repositories.Impl;

public class TicketTypeRepositoryImpl : ITicketTypeRepository
{
    private readonly IMongoCollection<Events.Models.Event> _collection;

    public TicketTypeRepositoryImpl(DatabaseSettings options)
    {
        var client = new MongoClient(options.ConnectionString);
        var database = client.GetDatabase(options.DatabaseName);
        _collection = database.GetCollection<Events.Models.Event>(options.EventCollectionName);
    }
    
    public TicketTypeRepositoryImpl(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<Events.Models.Event>(options.Value.EventCollectionName);
    }

    public async Task<TicketType> CreateAsync(string eventId, TicketType ticketType)
    {
        var filter = Builders<Events.Models.Event>.Filter.Eq(x => x.Id, eventId);

        var update = Builders<Events.Models.Event>.Update.Push(x => x.TicketTypes, ticketType);

        var projection = Builders<Events.Models.Event>.Projection
            .Include("TicketTypes");

        ticketType.Id = ObjectId.GenerateNewId().ToString();

        var updatedEvent = await _collection.FindOneAndUpdateAsync(
            filter: filter,
            update: update,
            new FindOneAndUpdateOptions<Events.Models.Event>
            {
                ReturnDocument = ReturnDocument.After,
                Projection = projection
            });


        var res = updatedEvent.TicketTypes.Find(x => x.Id == ticketType.Id);
        if (res is null)
        {
            throw new Exception("Error while creating ticket type");
        }

        return res;
    }

    public async Task<List<TicketType>> GetAllByEventId(string eventId)
    {
        var cursor = await _collection
            .FindAsync(
                filter: x => x.Id == eventId);
        var foundEvent = await cursor.ToListAsync();
        if (foundEvent.First() is null)
        {
            throw new Exception("Error while getting ticket type for given event");
        }

        return foundEvent.First().TicketTypes ?? [];
    }

    public async Task<TicketType?> GetByEventIdAndTicketTypeId(string eventId, string typeId)
    {
        var type = await _collection
            .Find(
                filter: x => x.Id == eventId && x.TicketTypes.Any(t => t.Id == typeId)
                )
            .FirstOrDefaultAsync();
        return type?.TicketTypes?.FirstOrDefault(x => x.Id == typeId);
        
    }

    public async Task<TicketType?> UpdateAsync(string eventId, string typeId, TicketType payload)
    {
        var ticketToUpdate = await GetByEventIdAndTicketTypeId(eventId, typeId);
        if (ticketToUpdate is null)
        {
            throw new Exception("Ticket type not found");
        }
        payload.Id = ticketToUpdate.Id;
        payload.LastModifiedDate = DateTime.Now;
        payload.CreatedDate = ticketToUpdate.CreatedDate;

        var filter = Builders<Events.Models.Event>.Filter.And(
            Builders<Events.Models.Event>.Filter.Eq(x => x.Id, eventId),
            Builders<Events.Models.Event>.Filter.Eq("TicketTypes.Id", typeId)
        );
            
        var update = Builders<Events.Models.Event>.Update.Set("TicketTypes.$", payload);
        
        var result = await _collection.UpdateOneAsync(filter : filter, update: update);

        if (result.MatchedCount != 1)
        {
            throw new Exception("Update failed. " + result.MatchedCount);
        }
        return payload;
        
    }
}