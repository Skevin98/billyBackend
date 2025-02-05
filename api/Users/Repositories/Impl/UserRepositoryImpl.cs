using api.Shared;
using api.Users.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Users.Repositories.Impl;

public class UserRepositoryImpl : IUserRepository
{
    private readonly IMongoCollection<UserEntity> _collection;

    public UserRepositoryImpl(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<UserEntity>(options.Value.UserCollectionName);
    }

    public UserRepositoryImpl(DatabaseSettings options)
    {
        var client = new MongoClient(options.ConnectionString);
        var database = client.GetDatabase(options.DatabaseName);
        _collection = database.GetCollection<UserEntity>(options.UserCollectionName);
    }

    public async Task<UserEntity> getUserById(string id)
    {
        var user = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        return user;
    }

    public async Task<List<UserEntity>> getUsersTicketsByEventId(string eventId)
    {
        var filter = Builders<UserEntity>.Filter.Eq("TicketsPurchased.EventId", eventId);
        var addFieldStage = new BsonDocument("$addFields", new BsonDocument
        {
            { "TicketsPurchased", new BsonArray { "$TicketsPurchased" }}
        });
        var res = await _collection.Aggregate()
            .Unwind<UserEntity,UserEntity>(x => x.TicketsPurchased)
            .Match(filter).AppendStage<UserEntity>(addFieldStage)
            .ToListAsync();

        return res;

    }
}