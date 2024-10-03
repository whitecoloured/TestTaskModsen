using EventsWebAPI.Core.Exceptions;
using Moq;
using Xunit;
using System.Threading;
using EventsWebAPI.Infrastructure.Repositories.Implementations;
using FluentValidation.Results;
using AutoMapper;
using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Application.Jwt.JwtTokenProviderService;
using System.Threading.Tasks;
using EventsWebAPI.UnitTests.Factory;
using EventsWebAPI.Application.Validation.Users;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Context;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Application.Commands_and_Queries.Users.Register;
using EventsWebAPI.Application.Commands_and_Queries.Users.Login;

namespace EventsWebAPI.UnitTests.Tests
{
    public class UserServicesTests
    {
        private readonly EventDbContext context;
        private readonly IUserRepository repo;
        private readonly Mock<CreateUserModelValidator> mockValidator;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<JwtTokenProviderService> mockJwtTokenProvider;
        public UserServicesTests()
        {
            context = new EventsDbContextFactory().CreateDbContext();
            repo = new UserRepository(context);
            mockValidator = new Mock<CreateUserModelValidator>();
            mockMapper = new Mock<IMapper>();
            mockJwtTokenProvider = new Mock<JwtTokenProviderService>();

        }
        [Fact]
        public async Task UserServices_RegisterTest_BadRequest()
        {
            //Arrange
            RegisterUserCommand Request = new("inna@", "Inna", "Demidovna", null);
            CancellationToken ct = new();
            mockValidator.Setup(v => v.Validate(Request)).Throws(new BadRequestException());

            var handler = new RegisterUserCommandHandler(repo, mockJwtTokenProvider.Object, mockValidator.Object, mockMapper.Object);
            
            //Act

            //Assert

            await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(Request,ct));
        }


        [Fact]
        public async Task UserServices_RegisterTest_NotAcceptable()
        {
            //Arrange
            var user = new User()
            {
                Name = "Роман",
                Surname = "Романович",
                Email = "roman@mail.ru"
            };
            context.Users.Add(user);
            context.SaveChanges();
            RegisterUserCommand Request = new("roman@mail.ru", "Роман", "Романенко", null);
            CancellationToken ct = new();

            var handler = new RegisterUserCommandHandler(repo, mockJwtTokenProvider.Object, mockValidator.Object, mockMapper.Object);
            //Act

            //Assert
            await Assert.ThrowsAsync<NotAcceptableException>(() => handler.Handle(Request, ct));
        }

        [Fact]
        public async Task UserServices_RegisterTest_Success()
        {
            //Arrange
            RegisterUserCommand Request = new("roman@mail.ru", "Роман", "Романович", null);
            CancellationToken ct = new();
            mockValidator.Setup(v => v.Validate(Request)).Returns(new ValidationResult());
            mockMapper.Setup(m => m.Map<User>(Request)).Returns(new User()
            {
                Name = "Роман",
                Surname = "Романович",
                Email = "roman@mail.ru",
                BirthDate=null,
                
            });

            var handler = new RegisterUserCommandHandler(repo, mockJwtTokenProvider.Object, mockValidator.Object, mockMapper.Object);

            //Act 
            var result = await handler.Handle(Request,ct);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UserServices_LoginTest_Success()
        {

            //Arrange
            var user = new User()
            {
                Name = "Роман",
                Surname = "Романович",
                Email = "roman@mail.ru"
            };
            context.Users.Add(user);
            context.SaveChanges();
            LoginUserCommand request = new("roman@mail.ru");
            CancellationToken ct = new();

            var handler = new LoginUserCommandHandler(repo, mockJwtTokenProvider.Object);

            //Act
            var result = await handler.Handle(request, ct);

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task UserServices_LoginTest_BadRequest()
        {
            var user = new User()
            {
                Name = "Роман",
                Surname = "Романович",
                Email = "roman@mail.ru"
            };
            context.Users.Add(user);
            context.SaveChanges();
            LoginUserCommand request = new("ron@mail.ru");
            CancellationToken ct = new();

            var handler = new LoginUserCommandHandler(repo, mockJwtTokenProvider.Object);


            //Assert
            await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(request, ct));
        }
    }
}
