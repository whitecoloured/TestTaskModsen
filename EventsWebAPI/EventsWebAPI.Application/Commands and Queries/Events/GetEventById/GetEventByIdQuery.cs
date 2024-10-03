using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetEventById
{
    public record GetEventByIdQuery([Required]Guid ID) : IRequest<GetEventByIdResponse>;
}
