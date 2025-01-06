using api.Shared;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Events.Repositories.Impl;

public class EventRepositoryImpl : IEventRepository
{
    private readonly IMongoCollection<Events.Models.Event> _collection;
    

    public EventRepositoryImpl(DatabaseSettings options)
    {
        var client = new MongoClient(options.ConnectionString);
        var database = client.GetDatabase(options.DatabaseName);
        _collection = database.GetCollection<Events.Models.Event>(options.EventCollectionName);
    }
    
    public EventRepositoryImpl(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<Events.Models.Event>(options.Value.EventCollectionName);
    }

    public async Task<Events.Models.Event>? GetByIdAsync(string i)
    {
        return await _collection.Find(e => e.Id == i).FirstOrDefaultAsync();
    }

    public async Task<List<Events.Models.Event>> GetByOwnerIdAsync(string owner)
    {
        return _collection.FindAsync(e => e.OwnerId == owner).Result.ToList();
    }

    public async Task<Events.Models.Event> CreateAsync(Events.Models.Event payload)
    {
        await _collection.InsertOneAsync(payload);
        var e = await _collection.Find(e => e.Id == payload.Id).FirstOrDefaultAsync();
        return e;
    }

    public async Task<Events.Models.Event?> UpdateAsync(string eventId, Events.Models.Event payload)
    {
        // await _collection.ReplaceOneAsync(e => e.Id == eventId, payload);

        var filter = Builders<Events.Models.Event>.Filter.Eq(x=>x.Id, eventId);
        var update = Builders<Events.Models.Event>.Update
            .Set("OwnerId", payload.OwnerId)
            .Set("StartDate", payload.StartDate)
            .Set("EndDate", payload.EndDate)
            .Set("Name", payload.Name)
            .Set("Description", payload.Description)
            .Set("LastModifiedDate", payload.LastModifiedDate)
            .Set("EventStatus", payload.EventStatus);

        var res = await _collection.UpdateOneAsync(filter, update);
        return _collection.Find(e => e.Id == eventId).FirstOrDefaultAsync().Result;
    }
}