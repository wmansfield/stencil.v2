using System;

namespace Placeholder.Common.Configuration
{
    public interface ISettingsResolver
    {
        string GetSetting(string name);
    }
}
