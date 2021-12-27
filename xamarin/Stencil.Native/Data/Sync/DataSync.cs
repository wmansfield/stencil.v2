using Stencil.Native.Commanding;
using Stencil.Native.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stencil.Native.Data.Sync
{
    public abstract class DataSync<TAPI> : TrackedClass<TAPI>, IDataSync
        where TAPI : StencilAPI
    {
        #region Constructor

        public DataSync(TAPI api, string trackPrefix)
            : base(api, trackPrefix)
        {
        }

        #endregion

        #region Properties

        public virtual bool Enabled { get; set; }
        public virtual bool AppIsActive { get; set; }

        public virtual Dictionary<string, DataDownloadJob> Jobs { get; protected set; }

        public virtual CancellationTokenSource CancellationTokenSource { get; protected set; }


        protected bool IsRunning { get; set; }

        #endregion

        #region Public Methods

        public virtual Task OnAppStartAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnAppStartAsync), async delegate ()
            {
                this.LogTrace("OnAppStarted");
                this.AppIsActive = true;

                await this.EnsureJobsAsync();

                this.StartProcessingJobsIfNeeded();
            });
        }

        public virtual Task OnAppResumeAsync()
        {
            return base.ExecuteFunction(nameof(OnAppResumeAsync), delegate ()
            {
                this.LogTrace("OnAppResume");
                this.AppIsActive = true;

                this.StartProcessingJobsIfNeeded();

                return Task.CompletedTask;
            });
        }

        public virtual Task OnAppSleepAsync()
        {
            return base.ExecuteFunction(nameof(OnAppSleepAsync), delegate ()
            {
                this.LogTrace("OnAppSleep");

                this.AppIsActive = false;

                CancellationTokenSource source = this.CancellationTokenSource;
                if (source != null)
                {
                    source.Cancel();
                }

                this.CancellationTokenSource = null;

                return Task.CompletedTask;
            });
        }

        public virtual Task OnSessionStartAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnSessionStartAsync), async delegate ()
            {
                await this.EnsureJobsAsync();

                this.StartProcessingJobsIfNeeded();
            });
        }
        
        public virtual Task OnSessionEndAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnSessionEndAsync), async delegate ()
            {
                CancellationTokenSource source = this.CancellationTokenSource;
                if (source != null)
                {
                    source.Cancel();
                }

                this.CancellationTokenSource = null;

                await this.EnsureJobsAsync();
            });
        }

        #endregion

        #region Protected Methods

        protected virtual void StartProcessingJobsIfNeeded()
        {
            base.ExecuteMethod(nameof(StartProcessingJobsIfNeeded), delegate ()
            {
                if (this.IsRunning)
                {
                    return;
                }
                Dictionary<string, DataDownloadJob> jobs = this.Jobs;
                if (jobs != null)
                {
                    this.IsRunning = true;
                    Task.Run(async delegate () 
                    {
                        try
                        {
                            await this.ProcessJobsAsync(jobs.Values.ToArray());
                        }
                        finally
                        {
                            this.IsRunning = false;
                        }
                    });
                }
            });
        }
        protected Task ProcessJobsAsync(DataDownloadJob[] dataDownloadJobs)
        {
            return base.ExecuteMethodAsync(nameof(ProcessJobsAsync), async delegate ()
            {
                dataDownloadJobs = dataDownloadJobs.OrderByDescending(x => x.Importance).ToArray();

                foreach (DataDownloadJob dataDownloadJob in dataDownloadJobs)
                {
                    if(!this.AppIsActive)
                    {
                        this.LogTrace("App is no longer active, cancelling data jobs");
                        break;
                    }

                    try
                    {
                        ScreenDownloadJob downloadScreenJob = dataDownloadJob as ScreenDownloadJob;
                        if(downloadScreenJob != null)
                        {
                            await this.ProcessScreenJobAsync(downloadScreenJob);
                        }
                        else
                        {
                            await this.ProcessGenericJobAsync(dataDownloadJob);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        this.LogTrace(string.Format("A Job was cancelled: {0}", dataDownloadJob.JobName));
                    }
                    catch (ThreadAbortException)
                    {
                        this.LogTrace(string.Format("A Job was aborted: {0}", dataDownloadJob.JobName));
                    }
                    catch (Exception ex)
                    {
                        this.LogError(ex, "ProcessJobsAsync:" + dataDownloadJob.JobName);
                    }
                }
            });
        }

        protected virtual Task ProcessGenericJobAsync(DataDownloadJob dataDownloadJob)
        {
            return base.ExecuteMethodAsync(nameof(ProcessGenericJobAsync), async delegate ()
            {
                if (dataDownloadJob.DownloadCommand != null)
                {
                    CommandScope scope = new CommandScope(this.API.CommandProcessor);
                    bool success = await this.API.CommandProcessor.ExecuteCommandAsync(scope, dataDownloadJob.DownloadCommand.CommandName, dataDownloadJob.DownloadCommand.CommandParameter);
                    if (success)
                    {
                        if (dataDownloadJob.DownloadSuccessCommand != null)
                        {
                            await this.API.CommandProcessor.ExecuteCommandAsync(scope, dataDownloadJob.DownloadSuccessCommand.CommandName, dataDownloadJob.DownloadSuccessCommand.CommandParameter);
                        }
                    }
                    else
                    {
                        if (dataDownloadJob.DownloadFailCommand != null)
                        {
                            await this.API.CommandProcessor.ExecuteCommandAsync(scope, dataDownloadJob.DownloadFailCommand.CommandName, dataDownloadJob.DownloadFailCommand.CommandParameter);
                        }
                    }
                }
            });
        }
        protected virtual Task ProcessScreenJobAsync(ScreenDownloadJob screenJob)
        {
            return base.ExecuteMethodAsync(nameof(ProcessScreenJobAsync), async delegate ()
            {
                if (screenJob.DownloadCommand != null && !string.IsNullOrWhiteSpace(screenJob.ScreenName))
                {
                    if(!screenJob.ForceDownload)
                    {
                        string storageKey = ScreenConfig.FormatStorageKey(screenJob.ScreenName, screenJob.ScreenParameter);
                        ScreenConfig currentConfig = await this.API.Screens.RetrieveScreenConfigAsync(storageKey, false);
                        if(currentConfig != null)
                        {
                            bool shouldDownload = this.ShouldDownload(screenJob.Lifetime, currentConfig.DownloadedUTC, currentConfig.ExpireUTC, currentConfig.CacheUntilUTC);
                            if(!shouldDownload)
                            {
                                //====---------------------------------------------------->>  Short Circuit, cache allowed
                                return;
                            }
                        }
                    }

                    CommandScope scope = new CommandScope(this.API.CommandProcessor);

                    NavigationData navigationData = new NavigationData()
                    {
                        screen_name = screenJob.ScreenName,
                        screen_parameter = screenJob.ScreenParameter
                    };
                    
                    object response = await this.API.CommandProcessor.ExecuteDataCommandAsync(scope, screenJob.DownloadCommand.CommandName, navigationData);
                    
                    ScreenConfig screenConfig = response as ScreenConfig;
                    
                    if (screenConfig != null)
                    {
                        await this.API.Screens.SaveScreenConfigAsync(screenConfig);
                        
                        if (screenJob.DownloadSuccessCommand != null)
                        {
                            await this.API.CommandProcessor.ExecuteCommandAsync(scope, screenJob.DownloadSuccessCommand.CommandName, screenJob.DownloadSuccessCommand.CommandParameter);
                        }
                    }
                    else
                    {
                        if (screenJob.DownloadFailCommand != null)
                        {
                            await this.API.CommandProcessor.ExecuteCommandAsync(scope, screenJob.DownloadFailCommand.CommandName, screenJob.DownloadFailCommand.CommandParameter);
                        }
                    }
                }
            });
        }

        protected virtual bool ShouldDownload(Lifetime lifeTime, DateTimeOffset? lastDownloadUTC, DateTimeOffset? expireUTC, DateTimeOffset? cacheUntilUTC)
        {
            return base.ExecuteFunction(nameof(ShouldDownload), delegate ()
            {
                switch (lifeTime)
                {
                    case Lifetime.until_expired:
                        bool isExpired = false;
                        if(expireUTC.HasValue)
                        {
                            isExpired = expireUTC.Value < DateTimeOffset.UtcNow;
                        }

                        bool cacheInValid = true;
                        if (cacheUntilUTC.HasValue)
                        {
                            cacheInValid = cacheUntilUTC.Value > DateTimeOffset.UtcNow;
                        }

                        return isExpired || cacheInValid;
                    default:
                        return true;
                }
            });
        }

        protected virtual Task EnsureJobsAsync()
        {
            return base.ExecuteMethodAsync(nameof(EnsureJobsAsync), async delegate ()
            {
                Dictionary<string, DataDownloadJob> currentJobs = this.Jobs;

                Dictionary<string, DataDownloadJob> prepareJobs = new Dictionary<string, DataDownloadJob>();

                if (currentJobs != null)
                {
                    foreach (KeyValuePair<string, DataDownloadJob> currentJob in currentJobs)
                    {
                        prepareJobs[currentJob.Key] = currentJob.Value;
                    }
                }

                Dictionary<string, DataDownloadJob> preparedJobs = await this.PrepareDownloadJobsAsync(prepareJobs);

                if (preparedJobs != null)
                {
                    Dictionary<string, DataDownloadJob> jobs = new Dictionary<string, DataDownloadJob>();
                    foreach (KeyValuePair<string, DataDownloadJob> job in preparedJobs)
                    {
                        if (!string.IsNullOrWhiteSpace(job.Key) && job.Value != null) // extra safety
                        {
                            jobs[job.Key] = job.Value;
                        }
                    }

                    this.Jobs = jobs;
                }
                else
                {
                    this.Jobs = null;
                }
            });
        }

        #endregion

        #region Abstract Methods
        
        /// <summary>
        /// Return jobs based on the current context, will replace existing jobs.
        /// Consider logged in user, existing data, etc.
        /// </summary>
        protected abstract Task<Dictionary<string, DataDownloadJob>> PrepareDownloadJobsAsync(Dictionary<string, DataDownloadJob> prepareJobs);

        #endregion
    }
}
