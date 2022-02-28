using System;
using RestSharp;

namespace Placeholder.SDK.Client
{
    public class RestRequestEx : RestRequest
    {
        public RestRequestEx(Method method)
            : base(method)
        {
        }
        public void AddParameter(string name, DateTime? dateTime)
        {
            base.AddParameter(name, dateTime.HasValue ? dateTime.Value.ToString("o") : null);
        }
    }
}
