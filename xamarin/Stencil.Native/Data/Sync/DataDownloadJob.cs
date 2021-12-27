using Stencil.Native.Commanding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Data.Sync
{
    /// <summary>
    /// Represents a job that the extending application desires to run.
    /// Note: Presumes command will handle persistence.
    /// </summary>
    public class DataDownloadJob
    {
        public string JobName { get; set; }
        public CommandInfo DownloadCommand { get; set; }
        public CommandInfo DownloadSuccessCommand { get; set; }
        public CommandInfo DownloadFailCommand { get; set; }
        public SyncPhase SyncPhase { get; set; }

        public int Importance { get; set; }
    }
}
