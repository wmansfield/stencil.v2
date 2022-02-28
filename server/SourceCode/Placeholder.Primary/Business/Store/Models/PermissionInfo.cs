using System;

namespace Placeholder.Primary.Business.Store
{
    public class PermissionInfo
    {
        public string token { get; set; }
        public Guid user_id { get; set; }
        public DateTime expires_utc { get; set; }
    }
}
