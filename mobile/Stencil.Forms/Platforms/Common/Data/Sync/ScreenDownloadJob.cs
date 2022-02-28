using Stencil.Common.Screens;
using Stencil.Forms.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Data.Sync
{
    /// <summary>
    /// Represents a download job that is persisted into Screen Manager.
    /// Jobs that do not fit this data model, should use DataDownloadJob or TrackedDownloadJob and persist separately
    /// </summary>
    public class ScreenDownloadJob : DataDownloadJob
    {
        public string ScreenName { get; set; }
        public string ScreenParameter { get; set; }
        public string NavigationData { get; set; }
        public bool ForceDownload { get; set; }
        public Lifetime Lifetime { get; set; }

        public TimeSpan? SyncAfterDuration { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
}
