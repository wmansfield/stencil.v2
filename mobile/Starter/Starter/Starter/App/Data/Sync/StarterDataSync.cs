using Stencil.Common.Screens;
using Stencil.Maui.Data.Sync;
using Stencil.Maui.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starter.App.Commands;
using Starter.App.Models;
using Starter.App.Data.Models;

namespace Starter.App.Data.Sync
{
    public class StarterDataSync : DataSync<StarterAPI>, IStarterDataSync
    {
        public StarterDataSync()
            : base(StarterAPI.Instance, nameof(StarterDataSync))
        {

        }


        public Task AgitateScreenDownloadAsync(NavigationData navigationData)
        {
            return base.ExecuteFunction("AgitateScreenDownloadAsync", delegate ()
            {
                if (navigationData != null && this.Jobs != null)
                {
                    string storageKey = ScreenConfig.FormatStorageKey(navigationData.screen_name, navigationData.screen_parameter);
                    ScreenDownloadJob match = this.Jobs.Values
                                                       .Where(x => x is ScreenDownloadJob)
                                                       .Select(x => x as ScreenDownloadJob)
                                                       .Where(x => ScreenConfig.FormatStorageKey(x.ScreenName, x.ScreenParameter) == storageKey)
                                                       .FirstOrDefault();
                    if (match != null)
                    {
                        this.AgitateJob(match.JobName);
                    }
                }
                return Task.CompletedTask;
            });
        }

        protected override Task<Dictionary<string, DataSyncJob>> PrepareDownloadJobsAsync(Dictionary<string, DataSyncJob> prepareJobs)
        {
            return base.ExecuteFunction("PrepareDownloadJobsAsync", delegate ()
            {
                // demonstrative
                //DataSyncJob settingDownload = this.GeneratSettingsFetchJob();
                //if (settingDownload != null)
                //{
                //    prepareJobs[settingDownload.JobName] = settingDownload;
                //}

                return Task.FromResult(prepareJobs);
            });
        }

        // demonstrative
        //protected DataSyncJob GeneratSettingsFetchJob()
        //{
        //    return base.ExecuteFunction("GeneratSettingsFetchJob", delegate ()
        //    {
        //        TrackedDownloadJob result = new TrackedDownloadJob()
        //        {
        //            JobName = WellKnownJobs.SETTINGS_FETCH,
        //            Lifetime = Lifetime.until_expired,
        //            SyncPhase = SyncPhase.AppResumed,
        //            Importance = 1000,
        //            EntityName = nameof(Setting),
        //            EntityIdentifier = "sync",
        //            Command = new CommandInfo()
        //            {
        //                CommandName = WellKnownCommands.SETTING_FETCH,
        //            }
        //        };

        //        return result;
        //    });
        //}

    }
}
