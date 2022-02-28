using System;

namespace Placeholder.Primary.Business.Synchronization
{
    public interface ISynchronizer
    {
        /// <summary>
        /// Used to notify health system which entity this synchronizer references
        /// </summary>
        string EntityName { get; }

        /// <summary>
        /// Executes a synchronization of items that are out of date
        /// </summary>
        /// <returns>The amount of records that were out of date</returns>
        [Obsolete("Only daemons should use this method", false)]
        int PerformSynchronization(string requestedAgentName, string tenant_code);

        int Priority { get; }

        void WaitForRefreshManual();

    }
}
