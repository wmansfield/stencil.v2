using System;

namespace Placeholder.SDK.Models.Requests
{
    public class AuthLoginInput
    {
        public AuthLoginInput()
        {
        }
        public string user { get; set; }
        public string password { get; set; }
        public string promotion { get; set; }
        public bool persist { get; set; }
        public Guid? account_id { get; set; }
    }
}
