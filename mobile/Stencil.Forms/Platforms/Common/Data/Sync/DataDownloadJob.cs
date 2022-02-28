using Stencil.Forms.Commanding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Data.Sync
{
    /// <summary>
    /// Represents a job that the extending application desires to run.
    /// Note: Presumes command will handle persistence, timing, parameters, etc.
    /// </summary>
    public class DataDownloadJob
    {
        public string JobName { get; set; }
        /// <summary>
        /// Note: The command should return the datatype expected by the caller. (designed as object for flexibility)
        /// </summary>
        public CommandInfo DownloadCommand { get; set; }
        public CommandInfo DownloadSuccessCommand { get; set; }
        public CommandInfo DownloadFailCommand { get; set; }
        public SyncPhase SyncPhase { get; set; }

        public int Importance { get; set; }
    }
}
