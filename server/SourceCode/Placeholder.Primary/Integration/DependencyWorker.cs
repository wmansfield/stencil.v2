using System;
using System.Threading;
using Placeholder.Domain;
using Placeholder.Primary.Daemons;
using Zero.Foundation;

namespace Placeholder.Primary.Business.Integration
{
    /// <summary>
    /// This is an abnormal pattern.
    /// TEntity is used to ensure a worker instance for each type, not used anywhere else
    /// </summary>
    public class DependencyWorker<TEntity> : WorkerBase<DependencyRequest>
    {
        /// <summary>
        /// Abornal pattern, takes the first processMethod and uses that for all instances
        /// This means a potential memory leak, so ensure the caller has the same lifetime as this instance.
        /// [done so that dependencies can be visualized in one shared place]
        /// </summary>
        public static void EnqueueRequest(IFoundation foundation, Dependency dependencies, Guid entity_id, Action<Dependency, Guid> processMethod)
        {
            DependencyWorker<TEntity> worker = EnqueueRequest<DependencyWorker<TEntity>>(foundation, WORKER_NAME, new DependencyRequest() { Dependencies = dependencies, EntityID = new IdentityInfo(entity_id) }, false);
            if (worker != null)
            {
                if (worker.ProcessMethod == null)
                {
                    worker.ProcessMethod = processMethod;
                    worker.Execute(worker.IFoundation, CancellationToken.None); // start it now (may have been waiting for processmethod)
                }
            }
        }
        /// <summary>
        /// Abornal pattern, takes the first processMethod and uses that for all instances
        /// This means a potential memory leak, so ensure the caller has the same lifetime as this instance.
        /// [done so that dependencies can be visualized in one shared place]
        /// </summary>
        public static void EnqueueRequest(IFoundation foundation, Dependency dependencies, Guid entity_id, Guid route_id, Action<Dependency, Guid, Guid> processMethod)
        {
            DependencyWorker<TEntity> worker = EnqueueRequest<DependencyWorker<TEntity>>(foundation, WORKER_NAME, new DependencyRequest() { Dependencies = dependencies, EntityID = new IdentityInfo(entity_id, route_id) }, false);
            if (worker != null)
            {
                if (worker.ProcessMethod == null)
                {
                    worker.ProcessMethodWithRoute = processMethod;
                    worker.Execute(worker.IFoundation, CancellationToken.None); // start it now (may have been waiting for processmethod)
                }
            }
        }

        public static string WORKER_NAME
        {
            get
            {
                return "Dependency_" + System.IO.Path.GetExtension(typeof(TEntity).ToString()).Trim('.');
            }
        }
        public DependencyWorker(IFoundation iFoundation)
            : base(iFoundation, WORKER_NAME)
        {
        }

        public Action<Dependency, Guid> ProcessMethod { get; set; }
        public Action<Dependency, Guid, Guid> ProcessMethodWithRoute { get; set; }

        protected override void ProcessRequests()
        {
            // prevent processing until we have an implementation
            if (this.ProcessMethod != null || this.ProcessMethodWithRoute != null)
            {
                base.ProcessRequests();
            }
        }
        protected override void ProcessRequest(DependencyRequest request)
        {
            base.ExecuteMethod("ProcessRequest", delegate ()
            {
                if (this.ProcessMethod != null)
                {
                    this.ProcessMethod(request.Dependencies, request.EntityID.primary_key);
                }
                if (this.ProcessMethodWithRoute != null)
                {
                    this.ProcessMethodWithRoute(request.Dependencies, request.EntityID.route_id.Value, request.EntityID.primary_key);
                }
            });
        }

    }
    public class DependencyRequest
    {
        public IdentityInfo EntityID { get; set; }
        public Dependency Dependencies { get; set; }
    }
}
