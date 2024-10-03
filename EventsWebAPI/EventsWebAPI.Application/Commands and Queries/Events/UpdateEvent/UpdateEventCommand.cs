using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.UpdateEvent
{
    public record UpdateEventCommand([Required]Guid ID, UpdateEventRequest Request) : IRequest;
}
