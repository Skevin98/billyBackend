using api.Shared;
using api.Tickets.Models;
using api.Tickets.Repositories;
using api.Tickets.Repositories.Impl;
using api.Tickets.Utils;
using api.Users.Models;
using test.Mocks;
using FluentAssertions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace test.UnitTests.repositories;

public class UnitTestsTicketEntityRepository: IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private ITicketEntityRepository _repository;
    private bool withMock = false;
    private TicketEntity dummyTicket;
    private const string eventId = "63f8ebd7e6b27d1f45ef05b5";
    private const string ticketTypeId = "677be50cd5238983a28e8a6b";
    private UserEntity dummyUser;
    private string dummyUserId = "6787f20c9a1a3e6163533ff3";
    private string ticketId = "67894f296c2baf05f617487d";

    public UnitTestsTicketEntityRepository(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var options = new DatabaseSettings();
        options.DatabaseName = "BillyDBTest";
        options.ConnectionString = "mongodb://localhost:27017";
        options.UserCollectionName = "Users";
        
        _repository = withMock
            ? new MockTicketEntityRepositoryImpl()
            : new TicketEntityRepositoryImpl(options);

        dummyTicket = new TicketEntity
        {
            Id = ticketId,
            TicketTypeId = ticketTypeId,
            EventId = eventId,
            Order = "T-0"
        };

        dummyUser = new UserEntity
        {
            Id = dummyUserId,
        };
    }

    public void Dispose() { }
    
    [Fact]
    [Trait("Category", "CREATE")]
    private async Task create_should_create_a_ticket()
    {
        var res = await _repository.Create(dummyUserId,dummyTicket);
        res.Id.Should().NotBeNull();
        res.Order.Should().NotBeNull();
        res.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(500));
    }

    [Fact]
    [Trait("Category", "CREATE")]
    private async Task create_should_create_a_ticket_with_event_and_type_infos()
    {
        var res = await _repository.Create(dummyUserId, dummyTicket);
        res.Id.Should().NotBeNull();
        res.EventId.Should().NotBeNull();
        res.TicketTypeId.Should().NotBeNull();
    }
    
    
    [Fact]
    [Trait("Category", "CREATE")]
    private async Task create_should_fail_if_user_does_not_exist()
    {
        Func<Task> action = async () => await _repository.Create("fake",dummyTicket);
        await action.Should().ThrowAsync<Exception>();
    }

    [Fact]
    [Trait("Category", "UPDATE")]
    private async Task update_ticket_should_change_its_status()
    {
        dummyTicket.Status = TicketStatus.REFUNDED;
        var updateTicket = await _repository.Update(dummyUserId, ticketId, dummyTicket);
        updateTicket.Status.Should().Be(TicketStatus.REFUNDED);
    }
    
    
    
}