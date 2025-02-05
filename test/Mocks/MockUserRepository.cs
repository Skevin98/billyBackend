using api.Users.Models;
using api.Users.Repositories;

namespace test.Mocks;

public class MockUserRepository : IUserRepository
{
    public Task<UserEntity> getUserById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserEntity>> getUsersTicketsByEventId(string eventId)
    {
        throw new NotImplementedException();
    }
}