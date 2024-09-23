using System;
using EventsWebAPI.Application.Dto_s.Requests.User;
using FluentValidation;

namespace EventsWebAPI.Application.Validation.Users
{
    public class CreateUserModelValidator : AbstractValidator<RegisterUserRequest>
    {
        public CreateUserModelValidator()
        {
            RuleFor(u => u.Name).NotEmpty().Length(3, 50);
            RuleFor(u => u.Surname).NotEmpty().Length(3, 50);
            RuleFor(u => u.Email).EmailAddress();
            When(u => u.BirthDate != null, () => {
                RuleFor(u => u.BirthDate).Must(d=> d.Value<DateTime.Now);
            });
        }
    }
}
