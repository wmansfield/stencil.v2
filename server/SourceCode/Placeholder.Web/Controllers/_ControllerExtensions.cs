using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Placeholder.Domain;
using Placeholder.Web.Security;
using RazorEngine;
using RazorEngine.Templating;
using Zero.Foundation.Web;
using sdk = Placeholder.SDK.Models;
using Placeholder.Common.Exceptions;
using System.Linq;
using System.Security.Claims;

namespace Placeholder.Web.Controllers
{
    public static class _ControllerExtensions
    {
        public static string GetRequestLanguage(this ControllerBase controller)
        {
            IRequestCultureFeature cultureFeature = controller?.Request?.HttpContext?.Features?.Get<IRequestCultureFeature>();
            if (cultureFeature?.RequestCulture?.UICulture?.Name != null)
            {
                return cultureFeature.RequestCulture.UICulture.Name;
            }
            return "en-US";
        }

        public static Account GetCurrentAccount(this PlaceholderApiController controller)
        {
            if(controller?.ControllerContext?.HttpContext?.Items != null && !controller.ControllerContext.HttpContext.Items.ContainsKey(SecurityAssumptions.CURRENT_ACCOUNT_HTTP_CONTEXT_KEY))
            {
                if (controller?.HttpContext?.User != null)
                {
                    Claim userClaim = controller.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                    if (userClaim != null)
                    {
                        Account account = controller.API.Direct.Accounts.GetByIdCached(new Guid(userClaim.Value));
                        if (account != null)
                        {
                            controller.HttpContext.Items[SecurityAssumptions.CURRENT_ACCOUNT_HTTP_CONTEXT_KEY] = account;
                        }
                    }
                }
            }
            
            if (controller?.ControllerContext?.HttpContext?.Items != null
                && controller.ControllerContext.HttpContext.Items.ContainsKey(SecurityAssumptions.CURRENT_ACCOUNT_HTTP_CONTEXT_KEY))
            {
                return controller.ControllerContext.HttpContext.Items[SecurityAssumptions.CURRENT_ACCOUNT_HTTP_CONTEXT_KEY] as Account;
            }
            return null;
        }

        public static void ValidateShopAdmin(this PlaceholderApiController controller, Guid shop_id)
        {
            controller.ValidateShopAdmin(shop_id, controller.GetCurrentAccount());
        }
        public static void ValidateShopAdmin(this PlaceholderApiController controller, Guid shop_id, Account account)
        {
            if (account == null || !controller.Security.IsShopRole(account, shop_id, sdk.ShopRole.Admin))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized)
                {
                    Content = "Unable to access required item"
                };
            }
        }

        public static void ValidateShopAnyRole(this PlaceholderApiController controller, Guid shop_id)
        {
            controller.ValidateShopAnyRole(shop_id, controller.GetCurrentAccount());
        }
        public static void ValidateShopAnyRole(this PlaceholderApiController controller, Guid shop_id, Account account)
        {
            if (account == null || !controller.Security.IsAnyShopRole(account, shop_id))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized)
                {
                    Content = "Unable to access required item"
                };
            }
        }

        public static bool CurrentClientPlatformIsMobile(this ControllerBase controller)
        {
            try
            {
                string platform = controller.GetClientPlatform();
                if(platform == null)
                {
                    return false;
                }
                return (platform.Contains("droid", StringComparison.OrdinalIgnoreCase) || platform.Contains("ios", StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                // gulp
            }
            return false;
        }

        public static string GetClientPlatform(this ControllerBase controller)
        {
            try
            {
                if (controller?.Request?.Headers != null
                    && controller.Request.Headers.ContainsKey(SecurityAssumptions.PARAM_PLATFORM))
                {
                    return controller.Request.Headers[SecurityAssumptions.PARAM_PLATFORM];
                }
            }
            catch
            {
                // gulp
            }

            return "web";
        }
        public static string GetClientPlatformVersion(this ControllerBase controller)
        {
            try
            {
                if (controller?.Request?.Headers != null
                    && controller.Request.Headers.ContainsKey(SecurityAssumptions.PARAM_VERSION))
                {
                    return controller.Request.Headers[SecurityAssumptions.PARAM_VERSION];
                }
            }
            catch
            {
                // gulp
            }

            return "2.0.0";
        }
        public static string GetRawBodyString(this ControllerBase controller, System.Text.Encoding encoding = null)
        {
            if (controller == null)
            {
                return null;
            }
            if (encoding == null)
            {
                encoding = System.Text.Encoding.UTF8;
            }

            using (StreamReader reader = new StreamReader(controller.Request.Body, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// cshtmlPath = ~/Views/Folder/Index.cshtml
        /// </summary>
        /// <returns></returns>
        public static string GenerateRazorResponse<T>(this PlaceholderApiController controller, string cshtmlPath, T model)
        {
            bool alreadyCached = true; // tricky.. but it works
            string cacheKey = string.Format("GenerateRazorResponse:{0}:{1}", model, cshtmlPath);
            alreadyCached = controller.SharedCache15.PerLifetime(cacheKey, delegate()
            {
                return !alreadyCached;
            });

            string html = string.Empty;
            if(alreadyCached)
            {
                html = Engine.Razor.Run(cacheKey,
                        typeof(T),
                        model);
            }
            else
            {
                IFileProvider provider = controller.ServiceProvider.GetService<IFileProvider>();
                IFileInfo file = provider.GetFileInfo(cshtmlPath);
                string razorTemplate = string.Empty;
                using (StreamReader reader = new StreamReader(file.CreateReadStream()))
                {
                    razorTemplate = reader.ReadToEnd();
                }

                html = Engine.Razor.RunCompile(razorTemplate,
                        cacheKey,
                        typeof(T),
                        model);
            }
            return html;
            
        }
    }
}
