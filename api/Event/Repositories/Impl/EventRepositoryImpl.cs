using api.Event.Repositories;
using api.Shared;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Event.Impl;

public class EventRepositoryImpl : IEventRepository
{
    private readonly IMongoCollection<Models.Event> _collection;
    

    public EventRepositoryImpl(DatabaseSettings options)
    {
        var client = new MongoClient(options.ConnectionString);
        var database = client.GetDatabase(options.DatabaseName);
        _collection = database.GetCollection<Models.Event>(options.EventCollectionName);
    }
    
    public EventRepositoryImpl(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<Models.Event>(options.Value.EventCollectionName);
    }

    public async Task<Models.Event>? GetByIdAsync(string i)
    {
        return await _collection.Find(e => e.Id == i).FirstOrDefaultAsync();
    }

    public async Task<List<Models.Event>> GetByOwnerIdAsync(string owner)
    {
        return _collection.FindAsync(e => e.OwnerId == owner).Result.ToList();
    }

    public async Task<Models.Event> CreateAsync(Models.Event payload)
    {
        await _collection.InsertOneAsync(payload);
        var e = await _collection.Find(e => e.Id == payload.Id).FirstOrDefaultAsync();
        return e;
    }

    public async Task<Models.Event?> UpdateAsync(string eventId, Models.Event payload)
    {
        await _collection.ReplaceOneAsync(e => e.Id == eventId, payload);
        return _collection.Find(e => e.Id == eventId).FirstOrDefaultAsync().Result;
    }
}