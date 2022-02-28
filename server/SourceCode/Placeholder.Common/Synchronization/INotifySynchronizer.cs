using System;

namespace Placeholder.Common.Synchronization
{
    public interface INotifySynchronizer
    {
        /// <summary>
        /// When an inline sync fails
        /// </summary>
        void OnSyncFailed(string type, string argument);

        /// <summary>
        /// Agitates specific daemon
        /// </summary>
        void AgitateDaemon(Guid? shop_id, string nameFormat);
        /// <summary>
        /// Agitates specific daemon
        /// </summary>
        void AgitateDaemon(string name);

        /// <summary>
        /// Agitates all daemons
        /// </summary>
        void AgitateSyncDaemon(Guid? shop_id);

    }
}
