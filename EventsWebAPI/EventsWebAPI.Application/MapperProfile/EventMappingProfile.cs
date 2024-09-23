using AutoMapper;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Application.Dto_s.Responses.Events;
using System;
using EventsWebAPI.Core.Enums;
using EventsWebAPI.Application.Dto_s.Requests.Event.Abstraction;

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

            CreateMap<Event, GetEventDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest=> dest.Description, opt=> opt.MapFrom(src=> src.Description))
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EventPlace, opt => opt.MapFrom(src => src.EventPlace))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest=> dest.MaxAmountOfMembers, opt=> opt.MapFrom(src=> src.MaxAmountOfMembers))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL));

            CreateMap<Event, EventInfoDto>()
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

            CreateMap<AttendingInfo, MemberEventDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Event.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate));

        }
    }
}
