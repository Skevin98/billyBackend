using api.Shared;
using api.Users.Models;
using Microsoft.Extensions.Options;
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
    public async Task<UserEntity> getUserById(string id)
    {
        var user = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        return user;
    }
}