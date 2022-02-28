using System;

namespace Placeholder.SDK.Models.Requests
{
    public class RegisterInput
    {
        public RegisterInput()
        {
        }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
    }
}
