using System;
using System.Text;

namespace Placeholder.SDK.Models
{
    public enum EmailTemplateKind
    {
        generic = 0,
        password_reset_started = 1,
        password_reset_completed = 2,
        email_changed = 2,
        email_verify_started = 2
    }
}
