using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebAPI.Core.Exceptions
{
    public class NotAcceptableException : Exception
    {
        public string message { get; } = "A Not Acceptable error has just occured.";

        public NotAcceptableException(string message)
        {
            this.message = message;
        }

        public NotAcceptableException()
        {

        }
    }
}
