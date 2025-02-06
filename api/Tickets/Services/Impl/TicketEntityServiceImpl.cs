using api.Shared.Validators;
using api.Tickets.Models;
using api.Tickets.Repositories;
using api.Tickets.Utils;
using api.Users.Models;
using api.Users.Repositories;
using FluentValidation;

namespace api.Tickets.Services.Impl;

public class TicketEntityServiceImpl : ITicketEntityService
{
    private ITicketEntityRepository _entityRepository;
    private ITicketTypeRepository _typeRepository;
    private IUserRepository _userRepository;

    public TicketEntityServiceImpl(ITicketEntityRepository entityRepository, ITicketTypeRepository ticketTypeRepository, IUserRepository userRepository)
    {
        _entityRepository = entityRepository;
        _typeRepository = ticketTypeRepository;
        _userRepository = userRepository;
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
            
            var userTickets = await getUsersTicketsByEventId(input.EventId);
            var ticketsForEvent = userTickets.SelectMany(ut => ut.TicketsPurchased);
            var ticketsIds = ticketsForEvent.ToList();
            if (isExistingTicketType == null || !ticketsIds.Select(tp=>tp.Id).Contains(input.Id))
            {
                throw new Exception("Ticket Type not found");
            }
            input.LastModifiedDate = DateTime.UtcNow;
            var payload = TicketEntity.FromInput(input);

            var ticketToUpdate = ticketsIds.FirstOrDefault(t => t.Id == input.Id);
            ThrowIfTicketsAlreadyControlled(ticketToUpdate, payload);

            var result = await _entityRepository.Update(userId,ticketId,payload);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new GraphQLException(e.Message);
        }

        void ThrowIfTicketsAlreadyControlled(TicketEntity? ticketToUpdate, TicketEntity payload)
        {
            string errorMessage = "Ticket Type already ";
            switch (ticketToUpdate.Status)
            {
                // TODO to use when event Owner configures his ticket to be checked once 
                // case TicketStatus.CHECKED when ticketToUpdate.Status.Equals(payload.Status):
                //     errorMessage += "checked";
                //     break;
                case TicketStatus.SOLD when ticketToUpdate.Status.Equals(payload.Status):
                    errorMessage += "sold";
                    break;
                case TicketStatus.CANCELED when ticketToUpdate.Status.Equals(payload.Status):
                    errorMessage += "canceled";
                    break;
                default:
                    return;
            }

            throw new GraphQLException(errorMessage);
        }
    }

    public async Task<List<UserEntity>> getUsersTicketsByEventId(string eventId)
    {
        try
        {
            var res = await _userRepository.getUsersTicketsByEventId(eventId);
            return res;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new GraphQLException(e.Message);
        }
    }
}