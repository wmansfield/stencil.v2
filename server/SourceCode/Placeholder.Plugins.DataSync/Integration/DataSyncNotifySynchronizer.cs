using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Placeholder.Common;
using Placeholder.Common.Configuration;
using Placeholder.Common.Synchronization;
using Placeholder.Plugins.DataSync.Daemons;
using Placeholder.Primary;
using Placeholder.Primary.Daemons;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;
using dm = Placeholder.Domain;

namespace Placeholder.Plugins.DataSync.Integration
{
    public class DataSyncNotifySynchronizer : ChokeableClass, INotifySynchronizer
    {
        public DataSyncNotifySynchronizer(IFoundation foundation)
            : base(foundation)
        {
            this.API = foundation.Resolve<PlaceholderAPI>();
            this.Cache15 = new AspectCache("DataSyncNotifySynchronizer", foundation, new ExpireStaticLifetimeManager("ElasticSyncNotifySynchronizer.Life15", System.TimeSpan.FromMinutes(15), false));
        }

        public AspectCache Cache15 { get; set; }
        public PlaceholderAPI API { get; set; }

        protected string DaemonUrl
        {
            get
            {
                return this.Cache15.PerLifetime("DaemonUrl", delegate ()
                {
                    if (this.API.Integration.Settings.IsLocalHost())
                    {
                        return "http://localhost:4337/";
                    }
                    return this.API.Direct.GlobalSettings.GetValueOrDefault(CommonAssumptions.APP_KEY_BACKING_URL, "https://placeholder-backing.foundationzero.com");
                });
            }
        }



        public void AgitateSyncDaemon(Guid? shop_id)
        {
            this.AgitateDaemon(shop_id, DataSynchronizeDaemon.DAEMON_NAME_TENANT_FORMAT);
        }
        public void AgitateDaemon(Guid? shop_id, string nameFormat)
        {
            base.ExecuteMethod(nameof(AgitateDaemon), delegate ()
            {
                string tenant = this.Cache15.PerLifetime(string.Format("shopCache:{0}", shop_id), delegate ()
                {
                    dm.Tenant tenant = this.API.Direct.Tenants.GetByShop(shop_id.GetValueOrDefault());
                    if(tenant != null)
                    {
                        return tenant.tenant_code;
                    }
                    return string.Empty;
                });
                this.AgitateDaemon(string.Format(nameFormat, tenant, Agents.AGENT_DEFAULT));
                this.AgitateDaemon(string.Format(nameFormat, tenant, Agents.AGENT_STATS));
            });
        }


        public void AgitateDaemon(string name)
        {
            base.ExecuteMethod(nameof(AgitateDaemon), delegate ()
            {
                try
                {
                    ISettingsResolver settingsResolver = this.IFoundation.Resolve<ISettingsResolver>();
                    bool isBackPlane = settingsResolver.IsBackPane();
                    if (isBackPlane)
                    {
                        WebHookProcessor.ProcessAgitateWebHook(this.IFoundation, "codeable", name);
                    }
                    else
                    {
                        Task.Run(delegate ()
                        {
                            string url = string.Format("{0}/api/datasync/agitate", this.DaemonUrl.Trim('/'));
                            string content = "key=codeable&name=" + name;
                            SendPostInNewThread(url, content);
                        });
                    }
                }
                catch (Exception ex)
                {
                    this.IFoundation.LogError(ex, "AgitateDaemon");
                }
            });
        }
        private void SendPostInNewThread(string url, string content)
        {
            Task.Run(delegate ()
            {
                try
                {
                    byte[] requestBytes = new ASCIIEncoding().GetBytes(content);

                    var request = WebRequest.CreateHttp(url);
                    request.Method = "POST";
                    request.ContentType = @"application/x-www-form-urlencoded";
                    request.ContentLength = content.Length;
                    var reqStream = request.GetRequestStream();
                    reqStream.Write(requestBytes, 0, requestBytes.Length);
                    reqStream.Close();
                    request.GetResponse();
                }
                catch
                {
                    // gulp
                }
            });
        }

        
    }
}
