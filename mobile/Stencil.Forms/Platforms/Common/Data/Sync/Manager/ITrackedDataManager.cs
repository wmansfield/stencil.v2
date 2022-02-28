using Stencil.Forms.Platforms.Common.Data.Sync;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Data.Sync.Manager
{
    public interface ITrackedDataManager
    {
        Task<TrackedDownloadInfo> RetrieveTrackedDownloadInfoAsync(string storageKey, bool includeExpired);
        Task SaveTrackedDownloadInfoAsync(TrackedDownloadInfo downloadInfo);
    }
}
