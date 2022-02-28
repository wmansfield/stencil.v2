using System;

namespace Placeholder.Primary.Integration
{
    public interface ILocalizer
    {
        string GetLocalized(string locale, string token, object[] arguments, string englishDefault);
    }
}
