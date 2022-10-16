using Stencil.Maui.Platforms.Common.Data.Sync;
using Stencil.Maui.Screens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui.Data.Sync.Manager
{
    public class TrackedDataManager<TAPI> : TrackedClass<TAPI>, ITrackedDataManager
        where TAPI : StencilAPI
    {
        #region Constructor

        public TrackedDataManager(TAPI api)
            : base(api, nameof(TrackedDataManager<TAPI>))
        {
        }

        #endregion

        #region Public Methods

        public virtual Task<TrackedDownloadInfo> RetrieveTrackedDownloadInfoAsync(string storageKey, bool includeExpired)
        {
            return base.ExecuteFunction(nameof(RetrieveTrackedDownloadInfoAsync), delegate ()
            {
                TrackedDownloadInfo result = null;
                using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                {
                    result = database.TrackedDownloadInfo_Get(storageKey);
                }
                if (!includeExpired)
                {
                    if (result != null)
                    {
                        if (result.ExpireUTC.HasValue && result.ExpireUTC.Value < DateTimeOffset.Now)
                        {
                            result = null; //TODO:MUST: How do we clean expired items?
                        }
                    }
                }

                return Task.FromResult(result);
            });
        }

        public virtual Task SaveTrackedDownloadInfoAsync(TrackedDownloadInfo downloadInfo)
        {
            return base.ExecuteFunction(nameof(SaveTrackedDownloadInfoAsync), delegate ()
            {
                string storageKey = TrackedDownloadInfo.FormatStorageKey(downloadInfo.EntityName, downloadInfo.EntityIdentifier);
                
                if(!string.IsNullOrWhiteSpace(storageKey))
                {
                    using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                    {
                        database.TrackedDownloadInfo_Upsert(downloadInfo);
                    }
                }
                
                return Task.CompletedTask;
            });
        }

        #endregion
    }
}
