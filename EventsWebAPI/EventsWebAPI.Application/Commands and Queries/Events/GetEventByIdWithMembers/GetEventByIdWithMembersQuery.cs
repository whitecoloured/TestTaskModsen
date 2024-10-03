using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetEventByIdWithMembers
{
    public record GetEventByIdWithMembersQuery([Required]Guid ID) : IRequest<EventInfoResponse>;
}
