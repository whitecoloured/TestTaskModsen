using System;
using System.Threading.Tasks;
using Moq;
using EventsWebAPI.Core.Enums;
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
using EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents;
using EventsWebAPI.Application.Commands_and_Queries.Events.CreateEvent;
using EventsWebAPI.Application.Commands_and_Queries.Events.UpdateEvent;

namespace EventsWebAPI.UnitTests.Tests
{
    public class EventServicesTests
    {
        private readonly EventDbContext context;
        private readonly IEventRepository repo;
        private readonly Mock<CreateEventModelValidator> mockCreateValidator;
        private readonly Mock<UpdateEventModelValidator> mockUpdateValidator;
        private readonly Mock<IMapper> mockMapper;

        public EventServicesTests()
        {
            context = new EventsDbContextFactory().CreateDbContext();
            repo = new EventRepository(context);
            mockCreateValidator = new Mock<CreateEventModelValidator>();
            mockUpdateValidator = new Mock<UpdateEventModelValidator>();
            mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task EventServices_GetAllEvents_Success()
        {
            //Arrange
            var events = GetEvents();
            context.Events.AddRange(events);
            context.SaveChanges();
            CancellationToken ct = new();

            GetAllEventsQuery query = new(null, null);

            var handler = new GetAllEventsQueryHandler(repo, mockMapper.Object);
            //Act

            var result = await handler.Handle(query, ct);

            //Assert

            Assert.NotEmpty(result.EventsList);
            Assert.Equal<int>(3, result.EventsAmount);
        }

        [Fact]
        public async Task EventServices_CreateEvent_BadRequest()
        {
            //Arrange
            CreateEventCommand request = new()
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

            var handler = new CreateEventCommandHandler(repo,mockCreateValidator.Object, mockMapper.Object);
            //Act

            //Assert
            await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(request, ct));
        }

        [Fact]
        public async Task EventServices_CreateEvent_NotAcceptable()
        {
            //Arrange
            var _event = GetEvent();
            context.Events.Add(_event);
            context.SaveChanges();
            CreateEventCommand request = new()
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

            var handler = new CreateEventCommandHandler(repo, mockCreateValidator.Object, mockMapper.Object);
            //Act

            //Assert
            await Assert.ThrowsAsync<NotAcceptableException>(() => handler.Handle(request, ct));
        }

        [Fact]
        public void EventServices_CreateEvent_Success()
        {
            //Arrange
            CreateEventCommand request = new()
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

            var handler = new CreateEventCommandHandler(repo, mockCreateValidator.Object, mockMapper.Object);

            //Act
            Task result = handler.Handle(request, ct);

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

            UpdateEventCommand command = new(ID, request);
            CancellationToken ct = new();

            mockUpdateValidator.Setup(v => v.Validate(request)).Throws(new BadRequestException());

            var handler = new UpdateEventCommandHandler(repo, mockUpdateValidator.Object, mockMapper.Object);

            //Act

            //Assert
            await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command,ct));
        }

        [Fact]
        public async Task EventServices_UpdateEvent_NotAcceptable()
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
                Name = "namename",
                Description = "descriptiondescription",
                EventDate = new DateTime(2024, 10, 19, 20, 00, 00),
                EventPlace = "ул. Окружная, 21",
                Category = EventCategory.Business,
                MaxAmountOfMembers = 3,
                ImageURL = null,
            };

            UpdateEventCommand command = new(ID, request);
            CancellationToken ct = new();

            mockUpdateValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            mockMapper.Setup(m => m.Map<Event>(request)).Returns(GetEventV2());


            //Act
            var handler = new UpdateEventCommandHandler(repo, mockUpdateValidator.Object, mockMapper.Object);


            //Assert
            await Assert.ThrowsAsync<NotAcceptableException>(() => handler.Handle(command, ct));
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

            UpdateEventCommand command = new(ID, request);
            CancellationToken ct = new();

            mockUpdateValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            mockMapper.Setup(m => m.Map<Event>(request)).Returns(GetEvent());

            var handler = new UpdateEventCommandHandler(repo, mockUpdateValidator.Object, mockMapper.Object);
            //Act
            Task result = handler.Handle(command, ct);

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
