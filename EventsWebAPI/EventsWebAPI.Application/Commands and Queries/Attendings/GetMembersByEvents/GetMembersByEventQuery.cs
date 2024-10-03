using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;


namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersByEvents
{
    public record GetMembersByEventQuery([Required]Guid ID): IRequest<List<EventMembersResponse>>;
}
