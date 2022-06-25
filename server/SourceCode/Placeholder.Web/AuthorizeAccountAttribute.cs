using Microsoft.AspNetCore.Authorization;
using Placeholder.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder
{
    public class AuthorizeAccountAttribute : AuthorizeAttribute
    {
        public AuthorizeAccountAttribute()
            : base()
        {
            this.AuthenticationSchemes = ApiAccountAuthenticationHandler.SCHEME;
        }
    }
}
