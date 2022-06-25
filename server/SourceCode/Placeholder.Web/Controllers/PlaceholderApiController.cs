using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Common;
using Placeholder.Common.Exceptions;
using Placeholder.Primary;
using Placeholder.Primary.Security;
using Placeholder.Web.Exceptions;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.System;
using Zero.Foundation.Unity;
using Zero.Foundation.Web;

namespace Placeholder.Web.Controllers
{
    /// <summary>
    /// A base class for an MVC controller without view support.
    /// </summary>
    [ApiController]
    public abstract class PlaceholderApiController : FoundationControllerBase
    {
        #region Constructors

        [Obsolete("Prefer IoC", true)]
        public PlaceholderApiController()
            : base()
        {
            this.API = CoreFoundation.Current.Resolve<PlaceholderAPI>();
            this.SharedCache15 = new AspectCache("PlaceholderApiController", this.IFoundation, new ExpireStaticLifetimeManager("PlaceholderApiController.Cache", TimeSpan.FromMinutes(15), false));
            this.SharedCache2 = new AspectCache("PlaceholderApiController2", this.IFoundation, new ExpireStaticLifetimeManager("PlaceholderApiController.Cache2", TimeSpan.FromMinutes(2), false));
        }
        public PlaceholderApiController(IFoundation iFoundation)
            : base(iFoundation)
        {
            this.API = iFoundation.Resolve<PlaceholderAPI>();
            this.SharedCache15 = new AspectCache("PlaceholderApiController", this.IFoundation, new ExpireStaticLifetimeManager("PlaceholderApiController.Cache", TimeSpan.FromMinutes(15), false));
            this.SharedCache2 = new AspectCache("PlaceholderApiController2", this.IFoundation, new ExpireStaticLifetimeManager("PlaceholderApiController.Cache2", TimeSpan.FromMinutes(2), false));
        }
        public PlaceholderApiController(IFoundation iFoundation, IHandleExceptionProvider iHandleExceptionProvider)
            : base(iFoundation, iHandleExceptionProvider)
        {
            this.API = iFoundation.Resolve<PlaceholderAPI>();
            this.SharedCache15 = new AspectCache("PlaceholderApiController", this.IFoundation, new ExpireStaticLifetimeManager("PlaceholderApiController.Cache", TimeSpan.FromMinutes(15), false));
            this.SharedCache2 = new AspectCache("PlaceholderApiController2", this.IFoundation, new ExpireStaticLifetimeManager("PlaceholderApiController.Cache2", TimeSpan.FromMinutes(2), false));

        }

        #endregion

        #region Properties

        public virtual PlaceholderAPI API { get; set; }
        /// <summary>
        /// Be sure to customize your keys!
        /// </summary>
        public virtual AspectCache SharedCache15 { get; set; }
        public virtual AspectCache SharedCache2 { get; set; }
        public IFoundation Foundation
        {
            get
            {
                return base.IFoundation;
            }
        }
        public ISecurityEnforcer Security
        {
            get
            {
                return this.API.Integration.Security;
            }
        }
        public IServiceProvider ServiceProvider
        {
            get
            {
                return this.IFoundation.SafeResolve<IServiceProvider>();
            }
        }

        #endregion

        #region Error Safety

        protected override T ExecuteFunction<T>(string methodName, Func<T> function, params object[] parameters)
        {
            try
            {
                return base.ExecuteFunction(methodName, function, parameters);
            }
            catch (Exception ex)
            {
                if (this.CurrentClientPlatformIsMobile())
                {
                    return (T)(object)this.Http200(new SDK.ActionResult()
                    {
                        success = false,
                        message = ex.Message
                    });
                }
                return (T)(object)this.Http400(ex.Message);
            }
        }
        protected override async Task<T> ExecuteFunctionAsync<T>(string methodName, Func<Task<T>> function, params object[] parameters)
        {
            try
            {
                return await base.ExecuteFunctionAsync(methodName, function, parameters);
            }
            catch (Exception ex)
            {
                if (this.CurrentClientPlatformIsMobile())
                {
                    return (T)(object)this.Http200(new SDK.ActionResult()
                    {
                        success = false,
                        message = ex.Message
                    });
                }
                return (T)(object)this.Http400(ex.Message);
            }
        }

#pragma warning disable 0108
        [Obsolete("Incorrect api call, use the Async Version of this method", true)]
        protected K ExecuteFunction<K>(string methodName, Func<Task<K>> function, params object[] parameters)
        {
            return base.ExecuteFunctionAsync(methodName, function, parameters).Result;
        }
#pragma warning restore 0108
        #endregion

        #region Validation

        protected virtual void ValidateNotNull<T>(Nullable<T> entity, string entityName)
            where T : struct
        {
            base.ExecuteMethod("ValidateNotNull", delegate ()
            {
                if (!entity.HasValue)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest)
                    {
                        Content = string.Format("Invalid {0} provided", entityName)
                    };
                }
            });
        }

        protected virtual void ValidateNotNull<T>(T entity, string entityName)
            where T : class
        {
            base.ExecuteMethod("ValidateNotNull", delegate ()
            {
                if (entity == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest)
                    {
                        Content = string.Format("Invalid {0} provided", entityName)
                    };
                }
            });
        }
        protected virtual void ValidateRouteMatch<T>(T routeId, T entityId, string entityName)
        {
            base.ExecuteMethod("ValidateRouteMatch", delegate ()
            {
                if (!routeId.Equals(entityId))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest)
                    {
                        Content = string.Format("Identifier mismatch for the {0} provided", entityName)
                    };
                }
            });
        }
        protected virtual void ValidateOffset(int value)
        {
            base.ExecuteMethod("ValidateOffset", delegate ()
            {
                if (value < 1)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest)
                    {
                        Content = "Offset must be greater than zero"
                    };
                }
            });
        }
        protected virtual void ValidateLimit(int value, int max)
        {
            base.ExecuteMethod("ValidateOffset", delegate ()
            {
                if ((value > max) || (value < 1))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest)
                    {
                        Content = string.Format("Limit must be between {0} and {1}", 1, max)
                    };
                }
            });
        }
        protected virtual void ThrowUnauthorized()
        {
            throw new HttpResponseException(HttpStatusCode.Unauthorized)
            {
                Content = "Unable to access required item"
            };
        }

        #endregion

        #region Http Results

        protected virtual IActionResult Http500(string reason)
        {
            return base.ExecuteFunction("Http500", delegate ()
            {
                return new ObjectResult(reason)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            });
        }
        protected virtual IActionResult Http400(string reason)
        {
            return base.ExecuteFunction("Http400", delegate ()
            {
                return new ObjectResult(reason)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            });
        }
        protected virtual IActionResult Http401(string reason)
        {
            return base.ExecuteFunction("Http401", delegate ()
            {
                return new ObjectResult(reason)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            });
        }
        protected virtual IActionResult Http404(string entityName)
        {
            return base.ExecuteFunction("Http404", delegate ()
            {
                return new ObjectResult(string.Format("Unable to find requested {0}", entityName))
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            });
        }
        protected virtual IActionResult Http200(object body = null)
        {
            return base.ExecuteFunction("Http200", delegate ()
            {
                return new ObjectResult(body)
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
            });
        }
        protected virtual IActionResult Http200(string htmlContent)
        {
            return base.ExecuteFunction("Http200", delegate ()
            {
                return new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = htmlContent,
                    ContentType = "text/html"
                };
            });
        }
        protected virtual IActionResult SimpleFileDownload(string fileName, string mimeType, string fullPath)
        {
            return base.ExecuteFunction("SimpleFileDownload", delegate ()
            {
                Stream stream = new FileStream(fullPath, FileMode.Open);
                return new FileStreamResult(stream, mimeType)
                {
                    FileDownloadName = fileName
                };
            });
        }
        protected virtual IActionResult SimpleFileDownload(string fileName, string mimeType, Stream stream)
        {
            return base.ExecuteFunction("SimpleFileDownload", delegate ()
            {
                return new FileStreamResult(stream, mimeType)
                {
                    FileDownloadName = fileName
                };
            });
        }
        protected virtual IActionResult SimpleFileDownload(string fileName, string mimeType, byte[] data)
        {
            return base.ExecuteFunction("SimpleFileDownload", delegate ()
            {
                return new FileContentResult(data, mimeType)
                {
                    FileDownloadName = fileName
                };
            });
        }
        protected virtual IActionResult SimpleFileInline(HttpResponse response, string fileName, string mimeType, byte[] data)
        {
            return base.ExecuteFunction("SimpleFileDownload", delegate ()
            {
                Response.Headers.Add("Content-Disposition", string.Format("inline; filename={0}", fileName));
                return new FileContentResult(data, mimeType);
            });
        }

        #endregion

        #region Localization

        protected Dictionary<string, object> Localize(Dictionary<string, LocalizableString> errors)
        {
            if (errors != null)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (var item in errors)
                {
                    result[item.Key] = this.Localize(item.Value);
                }
                return result;
            }
            return null;
        }
        protected Dictionary<string, object> Localize<TKey>(Dictionary<string, Dictionary<TKey, LocalizableString>> errors)
        {
            if (errors != null)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (var item in errors)
                {
                    Dictionary<TKey, LocalizableString> nestedErrors = item.Value;
                    Dictionary<TKey, object> nestedResult = new Dictionary<TKey, object>();
                    foreach (var nestedError in nestedErrors)
                    {
                        nestedResult[nestedError.Key] = this.Localize(nestedError.Value);
                    }
                    result[item.Key] = nestedResult;
                }
                return result;
            }
            return null;
        }
        protected string Localize(UIException exception)
        {
            if (exception != null)
            {
                return Localize(exception.LocalizableMessage);
            }
            return string.Empty;
        }
        protected string Localize(LocalizableString localizedString)
        {
            if (localizedString != null)
            {
                string language = this.GetRequestLanguage();
                return this.API.Integration.Localizer.GetLocalized(language, localizedString.Token, localizedString.Arguments, localizedString.DefaultText);
            }
            return string.Empty;
        }

        #endregion
    }
}
