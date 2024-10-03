using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.DeleteEvent
{
    public record DeleteEventCommand([Required]Guid ID) : IRequest;
}
