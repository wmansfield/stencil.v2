using Stencil.Maui.Screens;
using Realms;
using System;
using db = Stencil.Maui.Data.Models;
using System.Linq;
using System.Collections.Generic;
using Stencil.Maui.Platforms.Common.Data.Sync;

namespace Stencil.Maui.Data
{
    public abstract class StencilDatabase : TrackedClass, IStencilDatabase
    {
        #region Constructor

        public StencilDatabase(Realm realm)
            : base(nameof(StencilDatabase))
        {
            this.Realm = realm;
        }

        #endregion

        #region IDisposable

        private bool _hasDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_hasDisposed)
                {
                    _hasDisposed = true;
                    this.Realm = this.Realm.DisposeSafe();
                }
            }
            
        }
        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion

        #region Properties

        public Realm Realm { get; protected set; }

        #endregion

        #region Screen Methods
#if !DEBUG
        [Obsolete("Do not use in production.", true)]
#endif
        public List<ScreenConfig> ScreenConfig_All()
        {
            return base.ExecuteFunction(nameof(ScreenConfig_Get), delegate ()
            {
                return this.Realm.All<db.ScreenConfig>().ToUIModel();
            });
        }
        public ScreenConfig ScreenConfig_Get(string screenIdentifier)
        {
            return base.ExecuteFunction(nameof(ScreenConfig_Get), delegate ()
            {
                return this.Realm.Find<db.ScreenConfig>(screenIdentifier).ToUIModel();
            });
        }
        public void ScreenConfig_Upsert(ScreenConfig screenConfig)
        {
            base.ExecuteMethod(nameof(ScreenConfig_Upsert), delegate ()
            {
                Realm realm = this.Realm;
                realm.Write(() =>
                {
                    realm.Add(screenConfig.ToDbModel(), true);
                });
            });
        }
        public void ScreenConfig_Invalidate(string screenIdentifier)
        {
            base.ExecuteMethod(nameof(ScreenConfig_Invalidate), delegate ()
            {
                Realm realm = this.Realm;

                db.ScreenConfig screenConfig = realm.Find<db.ScreenConfig>(screenIdentifier);
                if(screenConfig != null)
                {
                    realm.Write(() =>
                    {
                        screenConfig.invalidated_utc = DateTimeOffset.UtcNow;
                    });
                }
            });
        }
        public void ScreenConfig_Remove(string screenIdentifier)
        {
            base.ExecuteMethod(nameof(ScreenConfig_Remove), delegate ()
            {
                Realm realm = this.Realm;

                db.ScreenConfig screenConfig = realm.Find<db.ScreenConfig>(screenIdentifier);
                if (screenConfig != null)
                {
                    realm.Write(() =>
                    {
                        realm.Remove(screenConfig);
                    });
                }
            });
        }

        public List<ScreenConfig> ScreenConfig_GetForDownloading()
        {
            return base.ExecuteFunction(nameof(ScreenConfig_Invalidate), delegate ()
            {
                Realm realm = this.Realm;

                List<ScreenConfig> screenConfigs = realm.All<db.ScreenConfig>()
                                                    .Where(x => x.automatic_download)
                                                    .ToUIModel();
                return screenConfigs;
            });
        }

        public List<ScreenConfig> ScreenConfig_GetWithName(string screen_name)
        {
            return base.ExecuteFunction(nameof(ScreenConfig_GetWithName), delegate ()
            {
                Realm realm = this.Realm;

                List<ScreenConfig> screenConfigs = realm.All<db.ScreenConfig>()
                                                    .Where(x => x.screen_name == screen_name)
                                                    .ToUIModel();
                return screenConfigs;
            });
        }

        #endregion


        #region Data 

        public TrackedDownloadInfo TrackedDownloadInfo_Get(string identifier)
        {
            return base.ExecuteFunction(nameof(TrackedDownloadInfo_Get), delegate ()
            {
                return this.Realm.Find<db.TrackedDownloadInfo>(identifier).ToUIModel();
            });
        }

        public void TrackedDownloadInfo_Upsert(TrackedDownloadInfo trackedDownloadInfo)
        {
            base.ExecuteMethod(nameof(TrackedDownloadInfo_Upsert), delegate ()
            {
                Realm realm = this.Realm;
                realm.Write(() =>
                {
                    realm.Add(trackedDownloadInfo.ToDbModel(), true);
                });
            });
        }

        #endregion
    }
}
