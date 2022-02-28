using System;

namespace Placeholder.Primary.Business.Synchronization
{
    public enum Availability
    {
        /// <summary>
        /// Will run asynchronous without waiting
        /// </summary>
        None,
        /// <summary>
        /// Whatever timeout policy the sychronizer has by default
        /// </summary>
        Default,
        /// <summary>
        /// Expect the process to complete with immediate capacity to be retrieved by id
        /// </summary>
        Retrievable,
        /// <summary>
        /// Expect the process to complete with immediate ability to find it in search.
        /// This will add ~850ms to the request to ensure adequate time for ES to refresh
        /// </summary>
        Searchable
    }
}
