using api.Shared;
using api.Tickets.Models;
using api.Users.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Tickets.Repositories.Impl;

public class TicketEntityRepositoryImpl : ITicketEntityRepository
{
    private readonly IMongoCollection<UserEntity> _collection;
    
    public TicketEntityRepositoryImpl(DatabaseSettings options)
    {
        var client = new MongoClient(options.ConnectionString); 
        var database = client.GetDatabase(options.DatabaseName);
        _collection = database.GetCollection<UserEntity>(options.UserCollectionName);
    }

    public TicketEntityRepositoryImpl(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString); 
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<UserEntity>(options.Value.UserCollectionName);
    }

    public async Task<TicketEntity> Create(string eventId, string ticketTypeId,string userId, TicketEntity payload)
    {
        var filter = Builders<UserEntity>.Filter.Eq(u => u.Id, userId);

        var update = Builders<UserEntity>.Update.Push(x => x.TicketsPurchased, payload);

        var projection = Builders<UserEntity>.Projection.Include(x => x.TicketsPurchased);
        
        payload.Id = ObjectId.GenerateNewId().ToString();
        
        var tickets = await _collection.FindOneAndUpdateAsync(
            filter: filter, 
            update : update,
            new FindOneAndUpdateOptions<UserEntity>
            {
                ReturnDocument = ReturnDocument.After,
                Projection = projection,
            });
        
        var res = tickets.TicketsPurchased.Find(x => x.Id == payload.Id);
        
        if (res is null)
        {
            throw new Exception("Error while creating ticket");
        }

        return res;

    }

    public Task<TicketEntity> Update(string ticketTypeId, string dummyUserId, string ticketId, TicketEntity payload)
    {
        throw new NotImplementedException();
    }
}