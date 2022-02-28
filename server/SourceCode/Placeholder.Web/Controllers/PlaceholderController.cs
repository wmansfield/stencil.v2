using System;
using System.Collections.Generic;
using Placeholder.Common;
using Placeholder.Common.Exceptions;
using Placeholder.Primary;
using Placeholder.Primary.Security;
using Zero.Foundation;
using Zero.Foundation.System;
using Zero.Foundation.Web;

namespace Placeholder.Web.Controllers
{
    /// <summary>
    /// A base class for an MVC controller with view support.
    /// </summary>
    public abstract class PlaceholderController : FoundationController
    {
        #region Constructors

        [Obsolete("Prefer IoC", true)]
        public PlaceholderController()
            : base()
        {
            this.API = CoreFoundation.Current.Resolve<PlaceholderAPI>();
        }
        public PlaceholderController(IFoundation iFoundation)
            : base(iFoundation)
        {
            this.API = iFoundation.Resolve<PlaceholderAPI>();
        }
        public PlaceholderController(IFoundation iFoundation, IHandleExceptionProvider iHandleExceptionProvider)
            : base(iFoundation, iHandleExceptionProvider)
        {
            this.API = iFoundation.Resolve<PlaceholderAPI>();
        }

        #endregion

        public virtual PlaceholderAPI API { get; set; }

        public IFoundation Foundation
        {
            get
            {
                return base.IFoundation;
            }
        }
        protected ISecurityEnforcer Security
        {
            get
            {
                return this.API.Integration.Security;
            }
        }

        

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
    }
}
