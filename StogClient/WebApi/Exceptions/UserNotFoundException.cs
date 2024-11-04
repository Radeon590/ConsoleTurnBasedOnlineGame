using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogClient.WebApi.Exceptions
{
    internal class UserNotFoundException: Exception
    {
        public UserNotFoundException(string username) : base($"user {username} not found") { }
    }
}
