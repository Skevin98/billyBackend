using api.Event.Services;
using api.Event.Services.Impl;
using api.Event.Utils;
using api.Events.Models;
using api.Events.Repositories;
using api.Events.Services;
using api.Shared;
using test.Mocks;
using FluentAssertions;
using FluentValidation;
using Xunit.Abstractions;

namespace test.IntegrationTests;

public class IntegrationtestsEvent : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private DatabaseSettings options;
    private IEventRepository _repository;
    private IEventService _service;
    private readonly string ownerId = "owner";
    private string? eventId = "6749b8d38f421d74b5787c1e";
    private string? badEventId = "fffcdffdbca3737b6158cccf";
    private EventInput eventInput;
    private EventInput badEventInput;

    public IntegrationtestsEvent(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        options = new DatabaseSettings();
        options.DatabaseName = "BillyDBTest";
        options.ConnectionString = "mongodb://localhost:27017";
        options.EventCollectionName = "Events";
        // _repository = new EventRepositoryImpl(options);
        _repository = new MockEventRepositoryImpl(); 
        _service = new EventServiceImpl(_repository);
        eventInput = new EventInput("new-owner",DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 
            "Fake Event", EventStatus.SCHEDULED,description: "Fake event description" );
        
        badEventInput = new EventInput(null,DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 
            "", EventStatus.SCHEDULED,description: "Fake event description" );
    }

    public void Dispose()
    {
        _repository = null;
    }

    [Fact]
    [Trait("Category", "GET")]
    public async Task Getting_events_by_owner_id_should_return_his_events()
    {
        var events = await _service.GetByOwnerId(ownerId);
        events.Should().NotBeNull().And.HaveCount(3);
    }
    
    [Fact]
    [Trait("Category", "GET")]
    public async Task Getting_events_by_owner_id_should_return_empty_list()
    {
        var events = await _service.GetByOwnerId("63f8e9b8a9a72b4c2fceffff");
        events.Should().NotBeNull().And.HaveCount(0);
    }
    
    [Fact]
    [Trait("Category", "CREATE")]
    public async Task Creating_event_should_create_event()
    {
        var ev = await _service.Create(eventInput);
        ev.Should().NotBeNull();
        ev.OwnerId.Should().Be(eventInput.OwnerId);
        _testOutputHelper.WriteLine(ev.Description);
    }
    
    [Fact]
    [Trait("Category", "CREATE")]
    public async Task Creating_event_should_have_exception_for_each_missing_infos()
    {
        Func<Task> act = async () => await _service.Create(badEventInput);
        await act.Should().ThrowAsync<ValidationException>();
       
        // var ev = await _service.Create(badEventInput);
        // ev.Should().BeNull();
        // ev.Errors.Should().NotBeNull().And.HaveCount(1);
        // ev.Errors?.ForEach(err => _testOutputHelper.WriteLine(err.Message));
    }
    
    [Fact]
    [Trait("Category", "UPDATE")]
    public async Task Update_event_should_update_event()
    {
        eventInput.Id = eventId;
        var ev = await _service.Update(eventInput, eventId);
        ev.Id.Should().Be(eventId);
        ev.Name.Should().Be(eventInput.Name);
        ev.LastModifiedDate.Should().NotBeNull();
    }
    
    [Fact]
    [Trait("Category", "UPDATE")]
    public async Task Update_event_should_have_exception_if_event_id_not_found()
    {
        Func<Task> act = async () => await _service.Update(eventInput, badEventId);
        await act.Should().ThrowAsync<KeyNotFoundException>();
       
        // eventInput.Id = badEventId;
        // var ev = await _service.Update(eventInput, badEventId);
        // ev.Errors.Should().NotBeNull().And.HaveCount(1);
        // ev.Errors[0].Message.Should().Be($"Event with Id {badEventId} not found.");
    }
    
    
}