using System;
using EventsWebAPI.Requests.User;
using System.Linq;
using FluentValidation;

namespace EventsWebAPI.Validation.Users
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
