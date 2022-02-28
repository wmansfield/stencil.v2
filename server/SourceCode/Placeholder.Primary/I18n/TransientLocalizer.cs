using System;
using Placeholder.Primary.Integration;
using Zero.Foundation;
using Zero.Foundation.Aspect;

namespace Placeholder.Primary.I18n
{
    public class TransientLocalizer : ChokeableClass, ILocalizer
    {
        public TransientLocalizer(IFoundation foundation)
            : base(foundation)
        {

        }

        public string GetLocalized(string locale, string token, object[] arguments, string englishDefault)
        {
            if (arguments != null && arguments.Length > 0)
            {
                return string.Format(englishDefault, arguments);
            }
            return englishDefault;
        }
    }
}
