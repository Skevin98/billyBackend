using api.Events.Models;
using api.Events.Repositories;
using api.Events.Services;
using api.Shared.Validators;
using FluentValidation;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace api.Events.Services.Impl;

public class EventServiceImpl : IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventServiceImpl(IEventRepository repository)
    {
        _eventRepository = repository;
    }

    public async Task<Events.Models.Event?> GetById(string eventId)
    {
        var ev = await _eventRepository.GetByIdAsync(eventId);
        return ev;
    }

    public async Task<List<Events.Models.Event>> GetByOwnerId(string ownerId)
    {
        var events = await _eventRepository.GetByOwnerIdAsync(ownerId);
        return events;
    }

    public async Task<Events.Models.Event> Create(EventInput eventInput)
    {
        try
        {
            EventInputValidator validator = new EventInputValidator();
            validator.ValidateAndThrow(eventInput);
            var evToCreate = new Events.Models.Event(eventInput);
            await _eventRepository.CreateAsync(evToCreate);
            var createdEvent = await _eventRepository.GetByIdAsync(evToCreate.Id);
            return createdEvent;
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e);
            throw new GraphQLException(e.Message);
        }
    }

    public async Task<Events.Models.Event> Update(EventInput eventInput, string eventId)
    {
        try
        {
            var evToUpdate = await _eventRepository.GetByIdAsync(eventId);
            if (evToUpdate == null)
                throw new KeyNotFoundException($"Event with Id {eventId} not found.");
            eventInput.Id = eventId;
            var input = new Events.Models.Event(eventInput);
            input.LastModifiedDate = DateTime.UtcNow;
            var updatedEvent = await _eventRepository.UpdateAsync(eventId, input);
            return updatedEvent;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}