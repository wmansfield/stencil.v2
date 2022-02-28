using System;
using System.Collections.Concurrent;
using Placeholder.Primary.Daemons;
using Zero.Foundation;

namespace Placeholder.Primary.Workers
{
    public class AccountLoggedInWorker : WorkerBase<LoggedInRequest>
    {
        public static void EnqueueRequest(IFoundation foundation, LoggedInRequest request)
        {
            EnqueueRequest<AccountLoggedInWorker>(foundation, WORKER_NAME, request, true, (int)TimeSpan.FromMinutes(2).TotalMilliseconds);
        }
        public const string WORKER_NAME = "AccountLoggedInWorker";

        public AccountLoggedInWorker(IFoundation iFoundation)
            : base(iFoundation, WORKER_NAME)
        {
            this.API = iFoundation.Resolve<PlaceholderAPI>();
        }

        public PlaceholderAPI API { get; set; }
        private ConcurrentDictionary<Guid, DateTime> _accountCache = new ConcurrentDictionary<Guid, DateTime>();

        public override void EnqueueRequest(LoggedInRequest request)
        {
            base.ExecuteMethod("EnqueueRequest", delegate ()
            {
                if (request.account_id.HasValue)
                {
                    _accountCache[request.account_id.Value] = request.login_utc;
                }
                
                this.RequestQueue.Enqueue(request);
                // we aren't agitating, we wait for the daemon to do a full time cycle
                //this.IFoundation.GetDaemonManager().StartDaemon(this.DaemonName);
            });
        }

        protected override void ProcessRequest(LoggedInRequest request)
        {
            base.ExecuteMethod("ProcessRequest", delegate ()
            {
                DateTime info = default(DateTime);
                if(request.account_id.HasValue)
                {
                    if (_accountCache.TryRemove(request.account_id.Value, out info))
                    {
                        if (info == default(DateTime))
                        {
                            info = DateTime.UtcNow;
                        }
                        this.API.Direct.Accounts.UpdateLastLogin(request.account_id.Value, info, request.platform);
                    }
                }
            });
        }
    }

    public class LoggedInRequest
    {
        public LoggedInRequest()
        {
        }
        public DateTime login_utc { get; set; }
        public Guid? account_id { get; set; }
        public Guid? shop_id { get; set; }
        public string platform { get; set; }
    }
}
