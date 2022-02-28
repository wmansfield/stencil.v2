using System;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Placeholder.Plugins.RestApi.Extensions
{
    public static class _ControllerExtensions
    {
        public static string GetCurrentLanguage(this ControllerBase controller)
        {
            IRequestCultureFeature cultureFeature = controller.Request.HttpContext.Features.Get<IRequestCultureFeature>();
            if(cultureFeature?.RequestCulture?.UICulture?.Name != null)
            {
                return cultureFeature.RequestCulture.UICulture.Name;
            }
            return "en-US";
        }
    }
}
