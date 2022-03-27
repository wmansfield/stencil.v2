using Stencil.Common.Screens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Data.Sync
{
    public interface IDataSync
    {
        bool Enabled { get; set; }

        Task OnAppStartAsync();
        Task OnAppResumeAsync();
        Task OnAppSleepAsync();
        Task OnSessionStartAsync();
        Task OnSessionEndAsync();

        bool ShouldDownload(Lifetime lifeTime, DateTimeOffset? lastDownloadUTC, DateTimeOffset? expireUTC, DateTimeOffset? cacheUntilUTC, DateTimeOffset? invalidatedUTC);
    }
}
