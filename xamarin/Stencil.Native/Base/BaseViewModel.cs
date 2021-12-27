using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Stencil.Native.Base
{
    public class BaseViewModel : TrackedClass, IBaseViewModel
    {
        #region Constructor

        public BaseViewModel(string trackPrefix)
            : base(trackPrefix)
        {
            this.AnalyticsScreen = trackPrefix;
        }

        #endregion

        #region Properties

        private string _viewTitle;
        public virtual string ViewTitle
        {
            get { return _viewTitle; }
            set { SetProperty(ref _viewTitle, value); }
        }

        private bool _isLoading;
        public virtual bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public virtual string AnalyticsScreen { get; protected set; }

        /// <summary>
        /// Has appeared at least once
        /// </summary>
        public virtual bool HasAppeared { get; protected set; }

        #endregion

        #region Commands

        private ICommand _navigateBackCommand;
        public ICommand NavigateBackCommand
        {
            get
            {
                return _navigateBackCommand ?? (_navigateBackCommand = new Command(async () => await this.NavigateBackAsync()));
            }
        }
        public virtual Task NavigateBackAsync()
        {
            return base.ExecuteFunction(nameof(NavigateBackAsync), delegate ()
            {
                return this.API.Router.PopViewAsync();
            });
        }

        #endregion

        #region Lifecycle Methods

        public virtual Task OnNavigatingToAsync()
        {
            return Task.CompletedTask;
        }
        public virtual Task OnNavigatingFromAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void OnAppear()
        {
            base.ExecuteMethod(nameof(OnAppear), delegate ()
            {
                this.TrackScreen();
                this.HasAppeared = true;
            });
        }
        public virtual void OnDisappear()
        {
        }

        #endregion

        #region Tracking Methods

        protected virtual void TrackScreen()
        {
            base.ExecuteMethod(nameof(TrackScreen), delegate ()
            {
                this.API.Analytics.TrackScreen(this.AnalyticsScreen);
            });
        }

        #endregion

        #region Resources

        protected Color StaticResourceColor(string key)
        {
            return base.ExecuteFunction(nameof(StaticResourceColor), delegate ()
            {
                return (Color)Application.Current.Resources[key];
            });
        }
        protected string StaticResourceFont(string key)
        {
            return base.ExecuteFunction(nameof(StaticResourceFont), delegate ()
            {
                string result = (string)(OnPlatform<string>)Application.Current.Resources[key];
                return result;
            });
        }

        #endregion
    }
}
