using api.Shared;
using api.Tickets.Models;
using MongoDB.Driver;

namespace api.Tickets.Repositories.Impl;

public class TicketEntityRepositoryImpl : ITicketEntityRepository
{
    private readonly IMongoCollection<TicketEntity> _collection;

    public TicketEntityRepositoryImpl(DatabaseSettings options)
    {
        var client = new MongoClient(options.ConnectionString); 
        var database = client.GetDatabase(options.DatabaseName);
        _collection = database.GetCollection<TicketEntity>(options.EventCollectionName);
    }

    public Task<TicketEntity> Create(string eventId, string ticketTypeId,string userId, TicketEntity dummyTicket)
    {
        throw new NotImplementedException();
    }

    public Task<TicketEntity> Update(string ticketTypeId, string dummyUserId, string ticketId, TicketEntity payload)
    {
        throw new NotImplementedException();
    }
}