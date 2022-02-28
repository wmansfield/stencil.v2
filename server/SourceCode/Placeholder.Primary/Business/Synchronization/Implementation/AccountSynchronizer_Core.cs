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
    public partial class AccountSynchronizer : SynchronizerBase<IdentityInfo>, IAccountSynchronizer
    {
        public AccountSynchronizer(IFoundation foundation)
            : base(foundation, "AccountSynchronizer")
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
                return 10;
            }
        }

        
        
        public override void PerformSynchronizationForItem(IdentityInfo identity)
        {
            base.ExecuteMethod("PerformSynchronizationForItem", delegate ()
            {
                Account domainModel = this.API.Direct.Accounts.GetById(identity.primary_key);
                if (domainModel != null)
                {
                    Action<Guid, bool, DateTime, string> synchronizationUpdateMethod = this.API.Direct.Accounts.SynchronizationUpdate;
                    if(this.API.Integration.Settings.IsHydrate())
                    {
                        synchronizationUpdateMethod = this.API.Direct.Accounts.SynchronizationHydrateUpdate;
                    }
                    DateTime syncDate = DateTime.UtcNow;
                    if (domainModel.sync_invalid_utc.HasValue)
                    {
                        syncDate = domainModel.sync_invalid_utc.Value;
                    }
                    try
                    {
                        sdk.Account sdkModel = domainModel.ToSDKModel();
                        this.SanitizeModel(sdkModel);
                        
                        this.HydrateSDKModelComputed(domainModel, sdkModel);
                        this.HydrateSDKModel(domainModel, sdkModel);

                        if (domainModel.deleted_utc.HasValue)
                        {
                            bool deleted = this.API.Store.Accounts.DeleteDocumentAsync(sdkModel).SyncResult();
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from store");
                            }
                            
                            synchronizationUpdateMethod(domainModel.account_id, true, syncDate, null);
                        }
                        else
                        {
                            string sync_log = string.Empty;
                            bool persisted = this.API.Store.Accounts.CreateDocumentAsync(sdkModel).SyncResult();
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to store");
                            }
                            sync_log += " Persisted to Store";
                            
                            synchronizationUpdateMethod(domainModel.account_id, true, syncDate, sync_log);
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
                    invalidItems = this.API.Direct.Accounts.SynchronizationHydrateGetInvalid(CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                else
                {
                    invalidItems = this.API.Direct.Accounts.SynchronizationGetInvalid(CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                foreach (IdentityInfo item in invalidItems)
                {
                    this.PerformSynchronizationForItem(item);
                }
                return invalidItems.Count;
            });
        }

        public sdk.Account AdHocHydrate(dm.Account domainModel)
        {
            return base.ExecuteFunction(nameof(AdHocHydrate), delegate ()
            {
                if(domainModel == null)
                {
                    return null;
                }
                else
                {
                    sdk.Account sdkModel = domainModel.ToSDKModel();
                    this.SanitizeModel(sdkModel);

                    this.HydrateSDKModelComputed(domainModel, sdkModel);
                    this.HydrateSDKModel(domainModel, sdkModel);

                    return sdkModel;
                }
            });
        }

        protected virtual void SanitizeModel(sdk.Account sdkModel)
        {
            
            sdkModel.email = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "email", sdkModel.email);
            sdkModel.first_name = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "first_name", sdkModel.first_name);
            sdkModel.last_name = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "last_name", sdkModel.last_name);
            sdkModel.account_display = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "account_display", sdkModel.account_display);
            sdkModel.password = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "password", sdkModel.password);
            sdkModel.password_salt = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "password_salt", sdkModel.password_salt);
            sdkModel.api_key = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "api_key", sdkModel.api_key);
            sdkModel.api_secret = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "api_secret", sdkModel.api_secret);
            sdkModel.timezone = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "timezone", sdkModel.timezone);
            sdkModel.email_verify_token = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "email_verify_token", sdkModel.email_verify_token);
            sdkModel.entitlements = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "entitlements", sdkModel.entitlements);
            sdkModel.password_reset_token = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "password_reset_token", sdkModel.password_reset_token);
            sdkModel.single_login_token = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "single_login_token", sdkModel.single_login_token);
            sdkModel.last_login_platform = PrimaryUtility.SanitizeHtml(this.API, "Account", sdkModel.account_id, "last_login_platform", sdkModel.last_login_platform);

        }
        
        /// <summary>
        /// Computed and Calculated Aggs, Typically Generated
        /// </summary>
        protected void HydrateSDKModelComputed(Account domainModel, sdk.Account sdkModel)
        {
            
        }
        partial void HydrateSDKModel(Account domainModel, sdk.Account sdkModel);
    }
}

