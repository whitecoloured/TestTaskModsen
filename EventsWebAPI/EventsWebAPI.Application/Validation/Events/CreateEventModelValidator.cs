using System;
using EventsWebAPI.Application.Dto_s.Requests.Event;
using FluentValidation;

namespace EventsWebAPI.Application.Validation.Events
{
    public class CreateEventModelValidator : AbstractValidator<CreateEventRequest>
    {
        public CreateEventModelValidator()
        {
            RuleFor(e => e.Name).NotEmpty()
                .Length(3, 30);

            RuleFor(e => e.Description).NotEmpty()
                .Length(10, 300);

            RuleFor(e => e.MaxAmountOfMembers).NotEmpty()
                .Must(amount => amount >= 3 && amount <= 10);

            RuleFor(e => ((int)e.Category)).Must(cat => cat >= 0 && cat <= 7);

            RuleFor(e => e.EventPlace).NotEmpty();

            RuleFor(e => e.EventDate).NotEmpty()
                .Must(d => d > DateTime.Now);
        }
    }
}
