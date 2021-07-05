using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AND.Models.Exceptions
{
    public class UserException : Exception
    {
        public UserException()
        {

        }

        public UserException(string message) : base(message)
        {

        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }
}
