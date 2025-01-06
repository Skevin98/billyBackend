using api.Events.Models;
using api.Shared;
using api.Tickets.Models;
using api.Tickets.Repositories;
using api.Tickets.Repositories.Impl;
using FluentAssertions;
using test.Mocks;

namespace test.UnitTests;

public class UnitTestsTicketTypeRepository : IDisposable
{
    private ITicketTypeRepository _typeRepository;
    private TicketType dummyTicket;
    private Event dummyEvent;
    private string eventId = "6749b8d38f421d74b5787c1e";
    private string fakeEventId = "6749b8d38f421d74b5787fff";
    private string typeId = "675b4477f96cb61f898ce956";
    private TicketType typePayload;
    private bool withMock = false;

    public UnitTestsTicketTypeRepository()
    {
        var options = new DatabaseSettings();
        options.DatabaseName = "BillyDBTest";
        options.ConnectionString = "mongodb://localhost:27017";
        options.EventCollectionName = "Events";
        _typeRepository = withMock
            ? new MockTicketTypeRepositoryImpl()
            : new TicketTypeRepositoryImpl(options);
        dummyTicket = new TicketType
        {
            Title = "",
            Description = "",
            Price = 100.0
        };
        
        typePayload = new TicketType
        {
            Title = "Modified Ticket",
            Description = "Modified description",
            Price = 500.0
        };
    }

    public void Dispose()
    {
        _typeRepository = null;
    }

    [Fact]
    [Trait("Category", "CREATE")]
    public async Task Create_billy_type_on_an_event_should_return_an_entity()
    {
        TicketType createdTicket = await _typeRepository.CreateAsync(eventId: eventId, dummyTicket);
        createdTicket.Should().NotBeNull();
        createdTicket.Id.Should().NotBeNull();
        createdTicket.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(500));
    }

    [Fact]
    [Trait("Category", "CREATE")]
    public async Task Create_billy_type_on_an_unknow_event_should_throw_an_exception()
    {
        Func<Task> action = async () => await _typeRepository.CreateAsync(eventId: fakeEventId, dummyTicket);
        await action.Should().ThrowAsync<Exception>();
    }

    [Fact]
    [Trait("Category", "GET")]
    public async Task Get_all_billy_type_on_an_event_should_return_all_entities()
    {
        var ticketTypes = await _typeRepository.GetAllByEventId(eventId: eventId);
        _typeRepository.Should().NotBeNull();
        ticketTypes.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    [Trait("Category", "GET")]
    public async Task Get_a_billy_type_by_id_on_an_event_should_return_the_type()
    {
        TicketType ticketType = await _typeRepository.GetByEventIdAndTicketTypeId(eventId: eventId, typeId : typeId);
        ticketType.Should().NotBeNull();
        ticketType.Id.Should().NotBeNull();
        ticketType.Price.Should().Be(500.0);
    }
    
    [Fact]
    [Trait("Category", "GET")]
    public async Task Get_a_billy_type_by_id_on_an_event_should_not_throw_an_exception()
    {
        Func<Task> action = async () => await _typeRepository.GetByEventIdAndTicketTypeId(eventId: fakeEventId
            , typeId: typeId);
        await action.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    [Trait("Category", "UPDATE")]
    public async Task Update_billy_type_on_an_event_should_return_an_entity()
    {
        TicketType updatedType = await _typeRepository.UpdateAsync(eventId: eventId, 
            typeId: typeId, payload: typePayload);
        updatedType.Should().NotBeNull();
        updatedType.Price.Should().Be(typePayload.Price);
        updatedType.LastModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(100));
        updatedType.CreatedDate.Should().NotBeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(500));
    }
    
    [Fact]
    [Trait("Category", "UPDATE")]
    public async Task Update_billy_type_on_an_unknow_event_should_throw_an_exception()
    {
        Func<Task> action = async () => await _typeRepository
            .UpdateAsync(eventId: fakeEventId,typeId: typeId, payload: typePayload);
        await action.Should().ThrowAsync<Exception>();
    }
    
    
    
    
}