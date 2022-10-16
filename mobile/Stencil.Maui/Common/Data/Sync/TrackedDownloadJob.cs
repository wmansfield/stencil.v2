using Stencil.Common.Screens;
using Stencil.Maui.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Data.Sync
{
    /// <summary>
    /// Represents a download job that is tracked but not persisted by stencil.
    /// Persistance of data is presumed to handled by the command itself.
    /// WARNING: The command that is coupled with this should return a TrackedDownloadInfo (signature is object)
    /// </summary>
    public class TrackedDownloadJob : DataSyncJob
    {
        public string EntityName { get; set; }
        public string EntityIdentifier { get; set; }
        public bool ForceDownload { get; set; }
        public Lifetime Lifetime { get; set; }
        public TimeSpan? SyncAfterDuration { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
}
