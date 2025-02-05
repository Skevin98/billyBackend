using api.Users.Models;

namespace api.Users.Repositories;

public interface IUserRepository
{
    public Task<UserEntity> getUserById(string id);
    public Task<List<UserEntity>> getUsersTicketsByEventId(string eventId);
}