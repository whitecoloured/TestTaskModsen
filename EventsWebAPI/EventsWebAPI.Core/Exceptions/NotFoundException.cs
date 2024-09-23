using System;

namespace EventsWebAPI.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public string message { get; } = "A Not Found error has just occured";
        public NotFoundException(string message)
        {
            this.message = message;
        }
        public NotFoundException()
        {

        }
    }
}
