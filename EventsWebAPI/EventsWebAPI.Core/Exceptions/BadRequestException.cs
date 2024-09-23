using System;

namespace EventsWebAPI.Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public string message { get; } = "A Bad Request error has just occured";
        public BadRequestException(string message)
        {
            this.message = message;
        }
        public BadRequestException()
        {

        }
    }
}
