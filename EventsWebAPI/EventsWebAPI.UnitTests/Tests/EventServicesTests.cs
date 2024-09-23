using System;
using System.Threading.Tasks;
using Moq;
using EventsWebAPI.Core.Enums;
using EventsWebAPI.Application.Services;
using EventsWebAPI.Application.Dto_s.Requests.Event;
using EventsWebAPI.Application.Dto_s.Responses.Events;
using EventsWebAPI.Context;
using EventsWebAPI.UnitTests.Factory;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Infrastructure.Repositories.Implementations;
using EventsWebAPI.Application.Validation.Events;
using AutoMapper;
using System.Collections.Generic;
using EventsWebAPI.Core.Models;
using Xunit;
using System.Threading;
using EventsWebAPI.Core.Exceptions;
using FluentValidation.Results;

namespace EventsWebAPI.UnitTests.Tests
{
    public class EventServicesTests
    {
        private readonly EventDbContext context;
        private readonly IEventRepository repo;
        private readonly Mock<CreateEventModelValidator> mockCreateValidator;
        private readonly Mock<UpdateEventModelValidator> mockUpdateValidator;
        private readonly Mock<IMapper> mockMapper;
        private readonly EventService service;

        public EventServicesTests()
        {
            context = new EventsDbContextFactory().CreateDbContext();
            repo = new EventRepository(context);
            mockCreateValidator = new Mock<CreateEventModelValidator>();
            mockUpdateValidator = new Mock<UpdateEventModelValidator>();
            mockMapper = new Mock<IMapper>();
            service = new EventService(repo, mockCreateValidator.Object, mockUpdateValidator.Object, mockMapper.Object);
        }

        [Fact]
        public async Task EventServices_GetAllEvents_Success()
        {
            //Arrange
            var events = GetEvents();
            context.Events.AddRange(events);
            context.SaveChanges();

            SearchEventsRequest searchRequest = new(null, null, null);
            FilterEventsRequest filterRequest = new(null, true);
            //Act

            var result = await service.GetAllEventsAsync(searchRequest, filterRequest, -1);

            //Assert

            Assert.NotEmpty(result.EventsList);
            Assert.Equal<int>(3, result.EventsAmount);
        }

        [Fact]
        public async Task EventServices_CreateEvent_BadRequest()
        {
            //Arrange
            CreateEventRequest request = new()
            {
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 09, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null
            };
            CancellationToken ct = new();

            mockCreateValidator.Setup(v => v.Validate(request)).Throws(new BadRequestException());
            //Act

            //Assert
            await Assert.ThrowsAsync<BadRequestException>(() => service.CreateEvent(request, ct));
        }

        [Fact]
        public async Task EventServices_CreateEvent_NotAcceptable()
        {
            //Arrange
            var _event = GetEvent();
            context.Events.Add(_event);
            context.SaveChanges();
            CreateEventRequest request = new()
            {
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null
            };
            CancellationToken ct = new();

            mockCreateValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            mockMapper.Setup(m => m.Map<Event>(request)).Returns(GetEvent());
            //Act

            //Assert
            await Assert.ThrowsAsync<NotAcceptableException>(() => service.CreateEvent(request, ct));
        }

        [Fact]
        public void EventServices_CreateEvent_Success()
        {
            //Arrange
            CreateEventRequest request = new()
            {
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 5,
                ImageURL = null
            };
            CancellationToken ct = new();

            mockCreateValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            mockMapper.Setup(m => m.Map<Event>(request)).Returns(GetEvent());

            //Act
            Task result = service.CreateEvent(request, ct);

            //Assert
            Assert.True(result.IsCompletedSuccessfully);
            
        }

        [Fact]
        public async Task EventServices_UpdateEvent_BadRequest()
        {
            //Arrange
            Guid ID = Guid.NewGuid();
            var _event = new Event()
            {
                Id=ID,
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null,
            };
            context.Events.Add(_event);
            context.SaveChanges();

            UpdateEventRequest request = new()
            {
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null
            };
            CancellationToken ct = new();

            mockUpdateValidator.Setup(v => v.Validate(request)).Throws(new BadRequestException());

            //Act

            //Assert
            await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateEvent(ID, request, ct));
        }

        [Fact]
        public async Task EventServices_UpdateEvent_NotAcceptable()
        {
            Guid ID = Guid.NewGuid();
            var _event = new Event()
            {
                Id = ID,
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null,
            };
            context.Events.Add(_event);
            context.SaveChanges();
            UpdateEventRequest request = new()
            {
                Name = "namename",
                Description = "descriptiondescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null,
            };
            CancellationToken ct = new();

            mockUpdateValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            mockMapper.Setup(m => m.Map<Event>(request)).Returns(GetEventV2());


            //Assert
            await Assert.ThrowsAsync<NotAcceptableException>(() => service.UpdateEvent(ID, request, ct));
        }

        [Fact]
        public void EventServices_UpdateEvent_Success()
        {
            //Arrange
            Guid ID = Guid.NewGuid();
            var _event = new Event()
            {
                Id = ID,
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null,
            };
            context.Events.Add(_event);
            context.SaveChanges();

            UpdateEventRequest request = new()
            {
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 10, 21, 10, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null
            };
            CancellationToken ct = new();

            mockUpdateValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            mockMapper.Setup(m => m.Map<Event>(request)).Returns(GetEvent());

            //Act
            Task result = service.UpdateEvent(ID, request, ct);

            //Assert
            Assert.True(result.IsCompletedSuccessfully);
        }

        private static Event GetEvent()
        {
            return new()
            {
                Name = "NameName",
                Description = "DescriptionDescription",
                EventDate = new DateTime(2024, 10, 21, 10, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null
            };
        }

        private static Event GetEventV2()
        {
            return new()
            {
                Name = "namename",
                Description = "descriptiondescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null,
            };
        }

        private static List<Event> GetEvents()
        {
            return new List<Event>()
            {
                new Event()
                {
                    Name="Event1",
                    Description="DescriptionEvent1",
                    EventDate=new DateTime(2024,10,02,20,00,00),
                    EventPlace="ул. Интернациональная 25",
                    Category=EventCategory.Games,
                    MaxAmountOfMembers=5,
                    ImageURL=null
                },
                new Event()
                {
                    Name="Event2",
                    Description="DescriptionEvent2",
                    EventDate=new DateTime(2024,10,03,12,00,00),
                    EventPlace="ул. Интернациональная 25",
                    Category=EventCategory.Films,
                    MaxAmountOfMembers=5,
                    ImageURL=null
                },
                new Event()
                {
                    Name="Event3",
                    Description="DescriptionEvent3",
                    EventDate=new DateTime(2024,10,10,10,00,00),
                    EventPlace="ул. Интернациональная 25",
                    Category=EventCategory.Games,
                    MaxAmountOfMembers=5,
                    ImageURL=null
                }
            };
        }

    }
}
