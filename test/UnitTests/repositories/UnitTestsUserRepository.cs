using api.Shared;
using api.Users.Repositories;
using api.Users.Repositories.Impl;
using FluentAssertions;
using test.Mocks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace test.UnitTests.repositories;

public class UnitTestsUserRepository : IDisposable
{
    private IUserRepository _userRepository;
    private bool withMock = false;
    private string eventId = "63f8e9b8a9a72b4c2fce01d1";
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTestsUserRepository(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var options = new DatabaseSettings();
        options.DatabaseName = "BillyDBTest";
        options.ConnectionString = "mongodb://localhost:27017";
        options.UserCollectionName = "Users";
        _userRepository = withMock 
            ? new MockUserRepository() 
            : new UserRepositoryImpl(options);
    }

    public void Dispose()
    {
    }


    [Fact]
    [Trait("Category", "GET")]
    private async Task get_tickets_by_event_id_should_return_tickets_list()
    {
        var res = await _userRepository.getUsersTicketsByEventId(eventId);
        _testOutputHelper.WriteLine(res.Count.ToString());
        res.Should().NotBeEmpty();
    }
    
    
}