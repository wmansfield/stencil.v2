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
    public partial class CompanySynchronizer : SynchronizerBase<IdentityInfo>, ICompanySynchronizer
    {
        public CompanySynchronizer(IFoundation foundation)
            : base(foundation, "CompanySynchronizer")
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
                Company domainModel = this.API.Direct.Companies.GetById(identity.route_id.Value, identity.primary_key);
                if (domainModel != null)
                {
                    Action<Guid, Guid, bool, DateTime, string> synchronizationUpdateMethod = this.API.Direct.Companies.SynchronizationUpdate;
                    if(this.API.Integration.Settings.IsHydrate())
                    {
                        synchronizationUpdateMethod = this.API.Direct.Companies.SynchronizationHydrateUpdate;
                    }
                    DateTime syncDate = DateTime.UtcNow;
                    if (domainModel.sync_invalid_utc.HasValue)
                    {
                        syncDate = domainModel.sync_invalid_utc.Value;
                    }
                    try
                    {
                        sdk.Company sdkModel = domainModel.ToSDKModel();
                        this.SanitizeModel(sdkModel);
                        
                        this.HydrateSDKModelComputed(domainModel, sdkModel);
                        this.HydrateSDKModel(domainModel, sdkModel);

                        if (domainModel.deleted_utc.HasValue)
                        {
                            bool deleted = this.API.Store.Companies.DeleteDocumentAsync(sdkModel).SyncResult();
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from store");
                            }
                            
                            synchronizationUpdateMethod(domainModel.shop_id, domainModel.company_id, true, syncDate, null);
                        }
                        else
                        {
                            string sync_log = string.Empty;
                            bool persisted = this.API.Store.Companies.CreateDocumentAsync(sdkModel).SyncResult();
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to store");
                            }
                            sync_log += " Persisted to Store";
                            
                            synchronizationUpdateMethod(domainModel.shop_id, domainModel.company_id, true, syncDate, sync_log);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.IFoundation.LogError(ex, "PerformSynchronizationForItem");
                        HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_ERROR_SYNC, this.EntityName), 0, 1);
                        synchronizationUpdateMethod(identity.route_id.Value, identity.primary_key, false, syncDate, FoundationUtility.FormatException(ex));
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
                    invalidItems = this.API.Direct.Companies.SynchronizationHydrateGetInvalid(tenant_code, CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                else
                {
                    invalidItems = this.API.Direct.Companies.SynchronizationGetInvalid(tenant_code, CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                foreach (IdentityInfo item in invalidItems)
                {
                    this.PerformSynchronizationForItem(item);
                }
                return invalidItems.Count;
            });
        }

        public sdk.Company AdHocHydrate(dm.Company domainModel)
        {
            return base.ExecuteFunction(nameof(AdHocHydrate), delegate ()
            {
                if(domainModel == null)
                {
                    return null;
                }
                else
                {
                    sdk.Company sdkModel = domainModel.ToSDKModel();
                    this.SanitizeModel(sdkModel);

                    this.HydrateSDKModelComputed(domainModel, sdkModel);
                    this.HydrateSDKModel(domainModel, sdkModel);

                    return sdkModel;
                }
            });
        }

        protected virtual void SanitizeModel(sdk.Company sdkModel)
        {
            
            sdkModel.company_name = PrimaryUtility.SanitizeHtml(this.API, "Company", sdkModel.company_id, "company_name", sdkModel.company_name);

        }
        
        /// <summary>
        /// Computed and Calculated Aggs, Typically Generated
        /// </summary>
        protected void HydrateSDKModelComputed(Company domainModel, sdk.Company sdkModel)
        {
            
        }
        partial void HydrateSDKModel(Company domainModel, sdk.Company sdkModel);
    }
}

