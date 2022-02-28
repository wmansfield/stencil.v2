using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Data.Models
{
    public class TrackedDownloadInfo : RealmObject, IDatabaseModel
    {
        public TrackedDownloadInfo()
        {
        }

        [PrimaryKey]
        public string id { get; set; }

        public string json { get; set; }
    }
}
