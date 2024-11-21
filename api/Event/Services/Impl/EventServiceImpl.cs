using api.Event.Models;
using api.Event.Models.Validators;
using api.Event.Repositories;
using api.Shared.Models;
using FluentValidation;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace api.Event.Services.Impl;

public class EventServiceImpl : IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventServiceImpl(IEventRepository repository)
    {
        _eventRepository = repository;
    }

    public async Task<Models.Event?> GetById(string eventId)
    {
        var ev = await _eventRepository.GetByIdAsync(eventId);
        return ev;
    }

    public async Task<List<Models.Event>> GetByOwnerId(string ownerId)
    {
        var events = await _eventRepository.GetByOwnerIdAsync(ownerId);
        return events;
    }

    public async Task<Models.Event> Create(EventInput eventInput)
    {
        try
        {
            EventInputValidator validator = new EventInputValidator();
            validator.ValidateAndThrow(eventInput);
            var evToCreate = new Models.Event(eventInput);
            await _eventRepository.CreateAsync(evToCreate);
            var createdEvent = await _eventRepository.GetByIdAsync(evToCreate.Id);
            return createdEvent;
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Models.Event> Update(EventInput eventInput, string eventId)
    {
        try
        {
            var evToUpdate = await _eventRepository.GetByIdAsync(eventId);
            if (evToUpdate == null)
                throw new KeyNotFoundException($"Event with Id {eventId} not found.");
            eventInput.Id = eventId;
            var input = new Models.Event(eventInput);
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