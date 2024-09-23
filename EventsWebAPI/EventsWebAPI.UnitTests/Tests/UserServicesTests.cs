using EventsWebAPI.Application.Services;
using EventsWebAPI.Core.Exceptions;
using Moq;
using Xunit;
using System.Threading;
using EventsWebAPI.Application.Dto_s.Requests.User;
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

namespace EventsWebAPI.UnitTests.Tests
{
    public class UserServicesTests
    {
        private readonly EventDbContext context;
        private readonly IUserRepository repo;
        private readonly Mock<CreateUserModelValidator> mockValidator;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<JwtDataProviderService> mockJwtDataProvider;
        private readonly Mock<JwtTokenProviderService> mockJwtTokenProvider;
        private readonly UserService service;
        public UserServicesTests()
        {
            context = new EventsDbContextFactory().CreateDbContext();
            repo = new UserRepository(context);
            mockValidator = new Mock<CreateUserModelValidator>();
            mockMapper = new Mock<IMapper>();
            mockJwtDataProvider = new Mock<JwtDataProviderService>();
            mockJwtTokenProvider = new Mock<JwtTokenProviderService>();
            service= new UserService(repo, mockValidator.Object, mockJwtTokenProvider.Object, mockJwtDataProvider.Object, mockMapper.Object);


        }
        [Fact]
        public async Task UserServices_RegisterTest_BadRequest()
        {
            //Arrange
            RegisterUserRequest Request = new("inna@", "Inna", "Demidovna", null);
            CancellationToken ct = new();
            mockValidator.Setup(v => v.Validate(Request)).Throws(new BadRequestException());

            
            //Act

            //Assert

            await Assert.ThrowsAsync<BadRequestException>(() => service.Register(Request, ct));
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
            RegisterUserRequest Request = new("roman@mail.ru", "Роман", "Романенко", null);
            CancellationToken ct = new();

            //Act

            //Assert
            await Assert.ThrowsAsync<NotAcceptableException>(() => service.Register(Request, ct));
        }

        [Fact]
        public async Task UserServices_RegisterTest_Success()
        {
            //Arrange
            RegisterUserRequest Request = new("roman@mail.ru", "Роман", "Романович", null);
            CancellationToken ct = new();
            mockValidator.Setup(v => v.Validate(Request)).Returns(new ValidationResult());
            mockMapper.Setup(m => m.Map<User>(Request)).Returns(new User()
            {
                Name = "Роман",
                Surname = "Романович",
                Email = "roman@mail.ru",
                BirthDate=null,
                
            });

            //Act 
            var result = await service.Register(Request, ct);

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
            LoginUserRequest request = new("roman@mail.ru");
            CancellationToken ct = new();

            //Act
            var result = await service.Login(request, ct);

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
            LoginUserRequest request = new("ron@mail.ru");
            CancellationToken ct = new();


            //Assert
            await Assert.ThrowsAsync<BadRequestException>(() => service.Login(request, ct));
        }
    }
}
