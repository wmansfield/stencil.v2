using Stencil.Common.Screens;
using Stencil.Maui.Data.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Data.Sync
{
    public interface IStarterDataSync : IDataSync
    {
        Task AgitateScreenDownloadAsync(NavigationData navigationData);
    }
}
