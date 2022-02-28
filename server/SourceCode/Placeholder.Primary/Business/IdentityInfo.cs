using System;

namespace Placeholder.Primary
{
    public class IdentityInfo
    {
        public IdentityInfo()
        {

        }
        public IdentityInfo(Guid primary_key)
        {
            this.primary_key = primary_key;
        }
        public IdentityInfo(Guid primary_key, Guid route_id)
        {
            this.route_id = route_id;
            this.primary_key = primary_key;
        }
        public Guid? route_id { get; set; }
        public Guid primary_key { get; set; }
    }
}
