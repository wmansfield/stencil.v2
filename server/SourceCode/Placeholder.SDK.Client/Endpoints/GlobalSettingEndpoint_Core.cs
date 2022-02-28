using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;

namespace Placeholder.SDK.Client.Endpoints
{
    public partial class GlobalSettingEndpoint : EndpointBase
    {
        public GlobalSettingEndpoint(PlaceholderSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<GlobalSetting>> GetGlobalSettingAsync(Guid global_setting_id)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "globalsettings/{global_setting_id}";
            request.AddUrlSegment("global_setting_id", global_setting_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<GlobalSetting>>(request);
        }

        public Task<ListResult<GlobalSetting>> Find(int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "globalsettings";
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            
            return this.Sdk.ExecuteAsync<ListResult<GlobalSetting>>(request);
        }

        public Task<ItemResult<GlobalSetting>> CreateGlobalSettingAsync(GlobalSetting globalsetting)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "globalsettings";
            request.AddJsonBody(globalsetting);
            return this.Sdk.ExecuteAsync<ItemResult<GlobalSetting>>(request);
        }

        public Task<ItemResult<GlobalSetting>> UpdateGlobalSettingAsync(Guid global_setting_id, GlobalSetting globalsetting)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "globalsettings/{global_setting_id}";
            request.AddUrlSegment("global_setting_id", global_setting_id.ToString());
            request.AddJsonBody(globalsetting);
            return this.Sdk.ExecuteAsync<ItemResult<GlobalSetting>>(request);
        }

        

        public Task<ActionResult> DeleteGlobalSettingAsync(Guid global_setting_id)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "globalsettings/{global_setting_id}";
            request.AddUrlSegment("global_setting_id", global_setting_id.ToString());
            
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
