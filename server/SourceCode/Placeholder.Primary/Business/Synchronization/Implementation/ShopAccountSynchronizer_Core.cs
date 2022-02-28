using Zero.Foundation;
using Placeholder.Common;
using Placeholder.Domain;
using Placeholder.Primary.Health;
using sdk = Placeholder.SDK.Models;
using dm = Placeholder.Domain;
using System;
using System.Collections.Generic;

namespace Placeholder.Primary.Business.Synchronization.Implementation
{
    public partial class ShopAccountSynchronizer : SynchronizerBase<IdentityInfo>, IShopAccountSynchronizer
    {
        public ShopAccountSynchronizer(IFoundation foundation)
            : base(foundation, "ShopAccountSynchronizer")
        {
        }

        public override int MillisecondsForRefresh
        {
            get
            {
                return 0; // Only ES needs this
            }
        }
        

        public override int Priority
        {
            get
            {
                return 30;
            }
        }

        
        
        public override void PerformSynchronizationForItem(IdentityInfo identity)
        {
            base.ExecuteMethod("PerformSynchronizationForItem", delegate ()
            {
                ShopAccount domainModel = this.API.Direct.ShopAccounts.GetById(identity.primary_key);
                if (domainModel != null)
                {
                    Action<Guid, bool, DateTime, string> synchronizationUpdateMethod = this.API.Direct.ShopAccounts.SynchronizationUpdate;
                    if(this.API.Integration.Settings.IsHydrate())
                    {
                        synchronizationUpdateMethod = this.API.Direct.ShopAccounts.SynchronizationHydrateUpdate;
                    }
                    DateTime syncDate = DateTime.UtcNow;
                    if (domainModel.sync_invalid_utc.HasValue)
                    {
                        syncDate = domainModel.sync_invalid_utc.Value;
                    }
                    try
                    {
                        sdk.ShopAccount sdkModel = domainModel.ToSDKModel();
                        this.SanitizeModel(sdkModel);
                        
                        this.HydrateSDKModelComputed(domainModel, sdkModel);
                        this.HydrateSDKModel(domainModel, sdkModel);

                        if (domainModel.deleted_utc.HasValue)
                        {
                            bool deleted = this.API.Store.ShopAccounts.DeleteDocumentAsync(sdkModel).SyncResult();
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from store");
                            }
                            
                            synchronizationUpdateMethod(domainModel.shop_account_id, true, syncDate, null);
                        }
                        else
                        {
                            string sync_log = string.Empty;
                            bool persisted = this.API.Store.ShopAccounts.CreateDocumentAsync(sdkModel).SyncResult();
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to store");
                            }
                            sync_log += " Persisted to Store";
                            
                            synchronizationUpdateMethod(domainModel.shop_account_id, true, syncDate, sync_log);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.IFoundation.LogError(ex, "PerformSynchronizationForItem");
                        HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_ERROR_SYNC, this.EntityName), 0, 1);
                        synchronizationUpdateMethod(identity.primary_key, false, syncDate, FoundationUtility.FormatException(ex));
                    }
                }
            });
        }
        
        public override int PerformSynchronization(string requestedAgentName, string tenant_code)
        {
            return base.ExecuteFunction("PerformSynchronization", delegate ()
            {
                string agentName = requestedAgentName;
                if(string.IsNullOrEmpty(agentName))
                {
                    agentName = this.AgentName;
                }
                List<IdentityInfo> invalidItems = new List<IdentityInfo>();

                if(this.API.Integration.Settings.IsHydrate())
                {
                    invalidItems = this.API.Direct.ShopAccounts.SynchronizationHydrateGetInvalid(CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                else
                {
                    invalidItems = this.API.Direct.ShopAccounts.SynchronizationGetInvalid(CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                foreach (IdentityInfo item in invalidItems)
                {
                    this.PerformSynchronizationForItem(item);
                }
                return invalidItems.Count;
            });
        }

        public sdk.ShopAccount AdHocHydrate(dm.ShopAccount domainModel)
        {
            return base.ExecuteFunction(nameof(AdHocHydrate), delegate ()
            {
                if(domainModel == null)
                {
                    return null;
                }
                else
                {
                    sdk.ShopAccount sdkModel = domainModel.ToSDKModel();
                    this.SanitizeModel(sdkModel);

                    this.HydrateSDKModelComputed(domainModel, sdkModel);
                    this.HydrateSDKModel(domainModel, sdkModel);

                    return sdkModel;
                }
            });
        }

        protected virtual void SanitizeModel(sdk.ShopAccount sdkModel)
        {
            
            sdkModel.shop_name = PrimaryUtility.SanitizeHtml(this.API, "ShopAccount", sdkModel.shop_account_id, "shop_name", sdkModel.shop_name);

        }
        
        /// <summary>
        /// Computed and Calculated Aggs, Typically Generated
        /// </summary>
        protected void HydrateSDKModelComputed(ShopAccount domainModel, sdk.ShopAccount sdkModel)
        {
            
            sdk.Shop referenceShop = null; // Index/Store Disabled, force direct
            
            if(referenceShop != null)
            {
                sdkModel.shop_name = referenceShop.shop_name;
            }
            else
            {
                Shop referenceDomainShop = this.API.Direct.Shops.GetById(sdkModel.shop_id);
                if(referenceDomainShop != null)
                {
                    sdkModel.shop_name = referenceDomainShop.shop_name;
                }
            }
            
        }
        partial void HydrateSDKModel(ShopAccount domainModel, sdk.ShopAccount sdkModel);
    }
}

