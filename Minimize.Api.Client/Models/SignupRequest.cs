using System;
using System.Collections.Generic;
using System.Text;

namespace Minimize.Api.Client.Models
{
    public class SignupRequest
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string password_confirmation { get; set; }
    }
}
