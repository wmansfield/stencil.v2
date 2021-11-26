using Stencil.Native.Screens;
using Realms;
using System;
using db = Stencil.Native.Data.Models;

namespace Stencil.Native.Data
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

        public ScreenConfig ScreenConfig_Get(string screenName)
        {
            return base.ExecuteFunction(nameof(ScreenConfig_Get), delegate ()
            {
                return this.Realm.Find<db.ScreenConfig>(screenName).ToUIModel();
            });
        }
        public void ScreenConfig_Upsert(string screenName, ScreenConfig screenConfig)
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

        #endregion
    }
}
