using api.Users.Models;

namespace api.Users.Services;

public interface IUserService
{
    public Task<UserEntity> getUserById(string id);
}