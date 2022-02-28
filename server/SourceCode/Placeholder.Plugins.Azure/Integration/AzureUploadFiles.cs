using Azure.Storage.Blobs;
using Placeholder.Common;
using Placeholder.Primary;
using Placeholder.Primary.Integration;
using Placeholder.SDK.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;

namespace Placeholder.Plugins.Azure.Integration
{
    public class AzureUploadFiles : ChokeableClass, IUploadFiles
    {
        public AzureUploadFiles(IFoundation foundation)
            : base(foundation)
        {
            this.API = foundation.Resolve<PlaceholderAPI>();
            this.ContainersEnsured = new ConcurrentDictionary<string, bool>();
            this.Clients = new ConcurrentDictionary<string, BlobContainerClient>();
            this.Cache15 = new AspectCache("AzureUploadFiles.15", foundation, new ExpireStaticLifetimeManager("AzureUploadFiles.15", TimeSpan.FromMinutes(2)));
        }

        public static object _SyncRoot = new object();

        public PlaceholderAPI API { get; set; }

        public AspectCache Cache15 { get; set; }

        public ConcurrentDictionary<string, bool> ContainersEnsured { get; set; }
        public ConcurrentDictionary<string, BlobContainerClient> Clients { get; set; }

        public UploadedFile UploadFile(Guid shop_id, byte[] bytes, string filePathAndName)
        {
            return base.ExecuteFunction(nameof(UploadFile), delegate ()
            {
                UploadedFile result = new UploadedFile();

                BlobContainerClient container = this.GenerateContainerClient(shop_id);

                BlobClient blob = container.GetBlobClient(filePathAndName);

                using(MemoryStream stream = new MemoryStream(bytes))
                {
                    blob.Upload(stream);
                    result.raw_url = blob.Uri.ToString();
                    result.public_url = blob.Uri.ToString();
                }

                return result;
            });
            
        }

        protected BlobContainerClient GenerateContainerClient(Guid shop_id)
        {
            return base.ExecuteFunction(nameof(GenerateContainerClient), delegate ()
            {
                string tenant_code = this.ResolveTenant(shop_id);

                BlobContainerClient client = null;
                this.Clients.TryGetValue(tenant_code, out client);

                if (client == null)
                {
                    lock (_SyncRoot)
                    {
                        this.Clients.TryGetValue(tenant_code, out client);
                        if (client == null)
                        {
                            client = new BlobContainerClient(this.GetConnectionString(tenant_code), CommonAssumptions.BLOB_ASSET_CONTAINER_NAME);

                            string containerEnsureKey = string.Format("{0}-{1}", tenant_code, CommonAssumptions.BLOB_ASSET_CONTAINER_NAME);
                            if (!this.ContainersEnsured.ContainsKey(containerEnsureKey))
                            {
                                client.CreateIfNotExists(global::Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                                this.ContainersEnsured.TryAdd(containerEnsureKey, true);
                            }

                            this.Clients[tenant_code] = client;
                        }
                    }
                }

                return client;
            });
        }

        protected virtual string ResolveTenant(Guid shop_id)
        {
            return base.ExecuteFunction(nameof(ResolveTenant), delegate ()
            {
                return this.Cache15.PerLifetime(string.Format("ResolveTenant{0}", shop_id), delegate ()
                {
                    Domain.Tenant tenant = this.API.Direct.Tenants.GetByShop(shop_id);
                    if (tenant != null)
                    {
                        return tenant.tenant_code;
                    }
                    return null;
                });
            });
        }
        
        protected string GetConnectionString(string tenant_code)
        {
            return base.ExecuteFunction(nameof(GetConnectionString), delegate ()
            {
                string key = string.Format(CommonAssumptions.APP_KEY_BLOB_CONNECTION_KEY_FORMAT, tenant_code);

                return this.API.Integration.Settings.GetSetting(key);
            });
        }

    }
}
