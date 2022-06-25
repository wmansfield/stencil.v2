using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class WidgetEndpoint : EndpointBase
    {
        public WidgetEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<Widget>> GetWidgetAsync(Guid shop_id, Guid widget_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "widgets/{shop_id}/{widget_id}";
            request.AddUrlSegment("shop_id", shop_id.ToString());
            request.AddUrlSegment("widget_id", widget_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<Widget>>(request);
        }

        

        public Task<ItemResult<Widget>> CreateWidgetAsync(Widget widget)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "widgets";
            request.AddJsonBody(widget);
            return this.Sdk.ExecuteAsync<ItemResult<Widget>>(request);
        }

        public Task<ItemResult<Widget>> UpdateWidgetAsync(Guid widget_id, Widget widget)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "widgets/{widget_id}";
            request.AddUrlSegment("widget_id", widget_id.ToString());
            request.AddJsonBody(widget);
            return this.Sdk.ExecuteAsync<ItemResult<Widget>>(request);
        }

        

        public Task<ActionResult> DeleteWidgetAsync(Guid shop_id, Guid widget_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "widgets/{shop_id}/{widget_id}";
            request.AddUrlSegment("widget_id", widget_id.ToString());
            request.AddUrlSegment("shop_id", shop_id.ToString());
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
