using Realms;
using Stencil.Native.Commanding;
using Stencil.Native.Data;
using Stencil.Native.Presentation.Routing;
using Stencil.Native.Screens;
using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Stencil.Native
{
    public abstract class NativeApplication<TAccount> : NativeApplication
    {
        #region Constructor

        public NativeApplication(string trackPrefix)
            : base(trackPrefix)
        {

        }

        #endregion
        public virtual TAccount CurrentAccount { get; protected set; }

    }
    public abstract class NativeApplication : TrackedClass
    {
        #region Constructor

        public NativeApplication(string trackPrefix)
            : base(trackPrefix)
        {

        }

        #endregion

        #region Static Container

        // dependency container seems wasteful for app singleton


        public static IStencilDatabaseConnector StencilDatabaseConnector { get; protected set; }
        public static IAppAnalytics Analytics { get; protected set; }
        public static ILogger Logger { get; protected set; }
        public static Application FormsApplication { get; protected set; }
        public static IRouter Router { get; protected set; }
        public static ICommandProcessor CommandProcessor { get; protected set; }
        public static IScreenManager ScreenManager { get; protected set; }
        public static NativeApplication Instance { get; protected set; }

        #endregion

        #region Properties

        protected virtual bool HasStarted { get; set; }

        protected abstract string InternalAppName { get; }

        #endregion

        #region Lifecycle

        public virtual Task OnStartAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnStartAsync), async delegate ()
            {
                if(!this.HasStarted)
                {
                    await this.EnsureEncryptionKeyAsync();
                    await this.NavigateToInitialPageAsync();
                    this.HasStarted = true;
                }
                //TODO:MUST: this.EnsureAppConfigDelayed();
                //TODO:MUST: this.EnsureAppVersionDelayed();
            });
        }

        public virtual Task OnResumeAsync()
        {
            return base.ExecuteFunction(nameof(OnResumeAsync), delegate ()
            {
                //TODO:MUST: this.EnsureAppConfigDelayed();
                //TODO:MUST: this.EnsureAppVersionDelayed();
                return Task.CompletedTask;
            });
        }

        public virtual Task OnSleepAsync()
        {
            return base.ExecuteFunction(nameof(OnSleepAsync), delegate ()
            {
                return Task.CompletedTask;
            });
        }

        public abstract void Outdated(string reason);

        protected abstract Task NavigateToInitialPageAsync();

        #endregion

        #region Database Methods

        protected ConcurrentDictionary<string, byte[]> _encryptionKeys;
        protected const string DB_KEY_GLOBAL = "global";

        public virtual Realm GenerateRealm(bool recreate = false)
        {
            return base.ExecuteFunction(nameof(GenerateRealm), delegate
            {
                byte[] encryptionKey = this.GetEncryptionKeySynchronous();
                RealmConfiguration configuration = new RealmConfiguration(string.Format("native.{0}.realm", this.InternalAppName.ToLower()))
                {
                    SchemaVersion = 1,
                    EncryptionKey = encryptionKey,
                    MigrationCallback = (migration, oldSchemaVersion) =>
                    {
                        //Use migration.NewRealm, migration.OldRealm for migrations that are more than adding a property.
                    }
                };
                if (recreate)
                {
                    try
                    {
                        Realm.DeleteRealm(configuration);
                    }
                    catch
                    {
                        // gulp
                    }
                }
                try
                {
                    return Realm.GetInstance(configuration);
                }
                catch
                {
                    // uh oh, try a new one
                    Realm.DeleteRealm(configuration);
                    return Realm.GetInstance(configuration);
                }
            });
        }

        public virtual Task<byte[]> EnsureEncryptionKeyAsync()
        {
            return base.ExecuteFunctionAsync(nameof(EnsureEncryptionKeyAsync), async delegate ()
            {
                if(_encryptionKeys == null)
                {
                    _encryptionKeys = new ConcurrentDictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
                }

                byte[] payload = null;
                if(!_encryptionKeys.TryGetValue(DB_KEY_GLOBAL, out payload) || payload == null || payload.Length == 0)
                {
                    byte[] keys = await this.EnsurePersistedEncryptionKeyAsync(DB_KEY_GLOBAL);
                    if (keys != null && keys.Length > 0)
                    {
                        _encryptionKeys.TryAdd(DB_KEY_GLOBAL, keys);
                    }

                    _encryptionKeys.TryGetValue(DB_KEY_GLOBAL, out payload);
                }
                return payload;
            });
        }

        protected virtual byte[] GetEncryptionKeySynchronous()
        {
            return base.ExecuteFunction(nameof(GetEncryptionKeySynchronous), delegate ()
            {
                if (_encryptionKeys != null)
                {
                    if(_encryptionKeys.TryGetValue(DB_KEY_GLOBAL, out byte[] result))
                    {
                        return result;
                    }
                }
                return null;
            });
        }
        
        protected virtual Task<byte[]> EnsurePersistedEncryptionKeyAsync(string name)
        {
            return base.ExecuteFunctionAsync(nameof(EnsurePersistedEncryptionKeyAsync), async delegate ()
            {
                byte[] encryptionKey = null;
                string hashedValue = await SecureStorage.GetAsync(CoreAssumptions.KEY_CRYPTO_DB_NAME);
                if (!string.IsNullOrEmpty(hashedValue))
                {
                    encryptionKey = Convert.FromBase64String(hashedValue);
                }
                if (encryptionKey == null)
                {
                    byte[] newKey = new Byte[64];
                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(newKey);
                    }

                    // sanity, check again
                    hashedValue = await SecureStorage.GetAsync(CoreAssumptions.KEY_CRYPTO_DB_NAME);
                    if (!string.IsNullOrEmpty(hashedValue))
                    {
                        encryptionKey = Convert.FromBase64String(hashedValue);
                    }
                    if (encryptionKey == null)
                    {
                        await SecureStorage.SetAsync(CoreAssumptions.KEY_CRYPTO_DB_NAME, Convert.ToBase64String(newKey));
                    }

                    // get back out
                    hashedValue = await SecureStorage.GetAsync(CoreAssumptions.KEY_CRYPTO_DB_NAME);
                    if (!string.IsNullOrEmpty(hashedValue))
                    {
                        encryptionKey = Convert.FromBase64String(hashedValue);
                    }
                }
                return encryptionKey;
            });
        }

        #endregion

    }
}
