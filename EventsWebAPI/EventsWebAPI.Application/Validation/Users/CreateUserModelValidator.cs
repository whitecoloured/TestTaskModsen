using System;
using EventsWebAPI.Application.Commands_and_Queries.Users.Register;
using FluentValidation;

namespace EventsWebAPI.Application.Validation.Users
{
    public class CreateUserModelValidator : AbstractValidator<RegisterUserCommand>
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
