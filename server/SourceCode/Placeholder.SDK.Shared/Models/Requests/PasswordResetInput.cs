using System;

namespace Placeholder.SDK.Models.Requests
{
    public class PasswordResetInput
    {
        public PasswordResetInput()
        {
        }
        public string email { get; set; }
        public string password { get; set; }
        public string token { get; set; }
    }
}
