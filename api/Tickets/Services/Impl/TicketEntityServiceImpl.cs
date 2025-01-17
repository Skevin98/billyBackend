using api.Shared.Validators;
using api.Tickets.Models;
using api.Tickets.Repositories;
using FluentValidation;

namespace api.Tickets.Services.Impl;

public class TicketEntityServiceImpl : ITicketEntityService
{
    private ITicketEntityRepository _entityRepository;
    private ITicketTypeRepository _typeRepository;

    public TicketEntityServiceImpl(ITicketEntityRepository entityRepository, ITicketTypeRepository ticketTypeRepository)
    {
        _entityRepository = entityRepository;
        _typeRepository = ticketTypeRepository;
    }

    public async Task<TicketEntity> Create(string userId, TicketEntityInput input)
    {
        try
        {
            var validator = new TicketEntityInputValidator();
            validator.ValidateAndThrow(input);
            var  ticketToCreate = TicketEntity.FromInput(input);
            var result = await _entityRepository.Create(userId,ticketToCreate);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new GraphQLException(e.Message);
        }
    }

    public  async Task<TicketEntity> Update(string userId, string ticketId, TicketEntityInput input)
    {
        try
        {
            var validator = new TicketEntityInputValidator();
            validator.ValidateAndThrow(input);
            var isExistingTicketType =  await _typeRepository
                .GetByEventIdAndTicketTypeId(input.EventId, input.TicketTypeId);
            if (isExistingTicketType == null)
            {
                throw new Exception("Ticket Type not found");
            }
            input.LastModifiedDate = DateTime.UtcNow;
            var payload = TicketEntity.FromInput(input);
            var result = await _entityRepository.Update(userId,ticketId,payload);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new GraphQLException(e.Message);
        }
    }
}