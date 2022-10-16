using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Data.Models
{
    public class TrackedDownloadInfo : RealmObject, IPersistedModel
    {
        public TrackedDownloadInfo()
        {
        }

        [PrimaryKey]
        public string id { get; set; }

        public string json { get; set; }
    }
}
