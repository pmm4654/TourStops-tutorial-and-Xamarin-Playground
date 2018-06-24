using System;
using System.Collections.Generic;
using System.Text;

namespace Minimize.Api.Client.Models
{
    public class SignupResponse
    {
        public string message { get; set; }
        public string AuthorizationToken { get; set; }
    }
}
