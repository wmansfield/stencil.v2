using Stencil.Maui.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Platforms.Common.Data.Sync
{
    public class TrackedDownloadInfo
    {
        public static string FormatStorageKey(string entityName, string entityIdentifier)
        {
            return string.Format("{0}.{1}", entityName, entityIdentifier).Trim().Trim('.');
        }

        public TrackedDownloadInfo()
        {

        }


        public string EntityName { get; set; }
        public string EntityIdentifier { get; set; }
        public DateTimeOffset? DownloadedUTC { get; set; }
        public DateTimeOffset? ExpireUTC { get; set; }
        public DateTimeOffset? CacheUntilUTC { get; set; }
        public DateTimeOffset? InvalidatedUTC { get; set; }
    }
}
