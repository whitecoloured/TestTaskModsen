using AutoMapper;
using EventsWebAPI.Core.Models;
using System;
using EventsWebAPI.Core.Enums;
using EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents.Responses;
using EventsWebAPI.Application.Commands_and_Queries.Events.GetEventById;
using EventsWebAPI.Application.Commands_and_Queries.Events.GetEventByIdWithMembers;
using EventsWebAPI.Application.Commands_and_Queries.Events.Abstraction;
using EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersEvents;

namespace EventsWebAPI.Application.MapperProfile
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<Event, ReadEventDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EventPlace, opt => opt.MapFrom(src => src.EventPlace))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.GetName(typeof(EventCategory), src.Category)))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL));

            CreateMap<Event, GetEventByIdResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest=> dest.Description, opt=> opt.MapFrom(src=> src.Description))
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EventPlace, opt => opt.MapFrom(src => src.EventPlace))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest=> dest.MaxAmountOfMembers, opt=> opt.MapFrom(src=> src.MaxAmountOfMembers))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL));

            CreateMap<Event, EventInfoResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EventPlace, opt => opt.MapFrom(src => src.EventPlace))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.MaxAmountOfMembers, opt => opt.MapFrom(src => src.MaxAmountOfMembers))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.Members.Count < src.MaxAmountOfMembers))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.EventDate < DateTime.Now));

            CreateMap<EventRequest, Event>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EventPlace, opt => opt.MapFrom(src => src.EventPlace))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.MaxAmountOfMembers, opt => opt.MapFrom(src => src.MaxAmountOfMembers))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL));

            CreateMap<AttendingInfo, MemberEventResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Event.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate));

        }
    }
}
