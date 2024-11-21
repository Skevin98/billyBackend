using api.Event.Impl;
using api.Event.Models;
using api.Event.Repositories;
using api.Shared;
using FluentAssertions;
using test.Mocks;
using Xunit.Abstractions;

namespace test.UnitTests;

public class UnitTestsEventRepository : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private IEventRepository _repository;
    private string name = "event1";
    private string description = "description";
    private string location = "location";
    private string eventType = "eventType";
    private DateTime startDate = DateTime.Now.AddDays(3);
    private DateTime endDate = DateTime.Now.AddDays(6);
    private string ownerId = "new-owner";
    private string eventId = "673cdffdbca3737b6158cccf";
    private string fakeEventId = "63f8e9b8a9a72b4c2fceffff";
    private Event newDummyEvent;
    private Event updateDummyEvent;
    private bool withMock = false;

    public UnitTestsEventRepository(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var options = new DatabaseSettings();
        options.DatabaseName = "BillyDBTest";
        options.ConnectionString = "mongodb://localhost:27017";
        options.EventCollectionName = "Events";

        _repository = withMock
            ? new MockEventRepositoryImpl()
            : new EventRepositoryImpl(options);
        
        newDummyEvent = new Event(null, ownerId,
            name, description,
            startDate, endDate);
        updateDummyEvent = new Event("673cdffdbca3737b6158cccf", ownerId,
            name, description,
            startDate, endDate, lastModifiedDate: DateTime.Now);
    }

    public void Dispose()
    {
        _repository = null;
    }


    [Fact]
    [Trait("Category", "GET")]
    public async Task Getting_by_id_should_returns_the_given_Event()
    {
        var event1 = await _repository.GetByIdAsync(withMock ? "1" : "63f8e9b8a9a72b4c2fce01d1");
        event1.Should().NotBeNull();
        event1.Id.Should().Be(withMock ? "1" : "63f8e9b8a9a72b4c2fce01d1");
    }

    [Fact]
    [Trait("Category", "GET")]
    public async Task Getting_by_id_should_return_null_if_event_not_found()
    {
        var event1 = await _repository.GetByIdAsync(withMock ? "7" : "63f8e9b8a9a72b4c2fceffff");
        
        event1.Should().BeNull();
        // Func<Task> act = async () => _repository.GetByIdAsync("7");
        // await act.Should().ThrowAsync<KeyNotFoundException>()
        //     .WithMessage($"Event with id {7} was not found");
    }

    [Fact]
    [Trait("Category", "GET")]
    public async Task Getting_by_owner_id_returns_the_list_of_events()
    {
        var events = await _repository.GetByOwnerIdAsync("owner");
        events.Should().HaveCount(3);
        foreach (var @event in events)
        {
            @event.OwnerId.Should().Be("owner");
        }
    }

    [Fact]
    [Trait("Category", "GET")]
    public async Task Getting_by_owner_id_with_event_returns_empty_list()
    {
        var events = await _repository.GetByOwnerIdAsync("no-event");
        events.Should().HaveCount(0);
    }

    [Fact]
    [Trait("Category", "CREATE")]
    public async Task Create_should_create_a_new_event_wih_given_name_and_description()
    {
        var payload = newDummyEvent;
        var event1 = await _repository.CreateAsync(payload);
        event1.Should().NotBeNull();
        _testOutputHelper.WriteLine(event1.Id);
        event1.Name.Should().Be(name);
        event1.Description.Should().Be(description);
        event1.Id.Should().NotBeNullOrEmpty();
    }

    [Fact]
    [Trait("Category", "CREATE")]
    public async Task Create_should_create_a_new_event_with_event_date()
    {
        var payload = newDummyEvent;
        var event1 = await _repository.CreateAsync(payload);
        event1.Should().NotBeNull();
        event1.StartDate.ToString().Should().Be(startDate.ToString());
        event1.EndDate.ToString().Should().Be(endDate.ToString());
        event1.Id.Should().NotBeNullOrEmpty();
    }

    [Fact]
    [Trait("Category", "CREATE")]
    public async Task Create_should_create_a_new_event_with_timestamps()
    {
        var payload = newDummyEvent;
        var event1 = await _repository.CreateAsync(payload);
        event1.Should().NotBeNull();
        event1.CreatedDate.Should().NotBeNull();
        event1.LastModifiedDate.Should().BeNull();
        event1.Id.Should().NotBeNullOrEmpty();
    }

    [Fact]
    [Trait("Category", "UPDATE")]
    public async Task Update_should_update_the_modified_date()
    {
        var payload = updateDummyEvent;
        var event1 = await _repository.UpdateAsync(eventId, payload);
        event1.Should().NotBeNull();
        event1.LastModifiedDate.ToString().Should().Be(updateDummyEvent.LastModifiedDate.ToString());
    }

    [Fact]
    [Trait("Category", "UPDATE")]
    public async Task Update_should_throw_if_event_not_found()
    {
        var payload = updateDummyEvent;
        var event1 = await _repository.UpdateAsync(fakeEventId, payload);
        event1.Should().BeNull();
        
        // Func<Task> act = async () => await _repository.UpdateAsync(fakeEventId, payload);
        // await act.Should().ThrowAsync<KeyNotFoundException>()
        //     .WithMessage($"Event with id {fakeEventId} was not found");
    }
}