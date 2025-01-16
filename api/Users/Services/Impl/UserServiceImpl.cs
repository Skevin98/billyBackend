using api.Users.Models;
using api.Users.Repositories;

namespace api.Users.Services.Impl;

public class UserServiceImpl : IUserService
{
    private readonly IUserRepository _repository;

    public UserServiceImpl(IUserRepository userRepository)
    {
        _repository = userRepository;
    }
    public async Task<UserEntity> getUserById(string id)
    {
        return await _repository.getUserById(id);
    }
}