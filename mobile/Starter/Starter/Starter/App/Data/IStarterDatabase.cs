using Stencil.Maui.Data;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using Starter.App.Models;
using db = Starter.App.Data.Models;

namespace Starter.App.Data
{
    public interface IStarterDatabase : IStencilDatabase
    {
        Self SelfRetrieve();
        Self SelfUpsert(Self self);
        Self SelfUpdate(Action<Self> operation);

        string SettingRetrieve(string key, string defaultValue);

        void SettingUpsert(string key, string value);
    }
}
