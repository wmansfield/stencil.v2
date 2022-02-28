using System.Threading.Tasks;

namespace Stencil.Forms.Base
{
    public abstract class BaseViewModelPage<TViewModel> : BaseContentPage
        where TViewModel : BaseViewModel
    {
        public BaseViewModelPage(string trackPrefix, TViewModel viewModel)
            : base(trackPrefix)
        {
        }

        #region Properties

        public virtual BaseViewModel BaseViewModel { get; protected set; }

        public bool DisableAppearanceLifeCycle { get; set; }

        #endregion

        #region Lifecycle

        public virtual Task OnNavigatingToAsync()
        {
            return this.ExecuteFunction(nameof(OnNavigatingToAsync), delegate ()
            {
                if (!this.DisableAppearanceLifeCycle)
                {
                    if(this.BaseViewModel != null)
                    {
                        return this.BaseViewModel.OnNavigatingToAsync();
                    }
                }
                return Task.CompletedTask;
            });
        }
        public virtual Task OnNavigatedToAsync()
        {
            return this.ExecuteFunction(nameof(OnNavigatedToAsync), delegate ()
            {
                if (!this.DisableAppearanceLifeCycle)
                {
                    if (this.BaseViewModel != null)
                    {
                        return this.BaseViewModel.OnNavigatedToAsync();
                    }
                }
                return Task.CompletedTask;
            });
        }
        public virtual Task OnNavigatingFromAsync()
        {
            return this.ExecuteFunction(nameof(OnNavigatingFromAsync), delegate ()
            {
                if (!this.DisableAppearanceLifeCycle)
                {
                    if (this.BaseViewModel != null)
                    {
                        return this.BaseViewModel?.OnNavigatingFromAsync();
                    }
                }
                return Task.CompletedTask;
            });
        }

        protected override void OnAppearing()
        {
            this.ExecuteMethod(nameof(OnAppearing), delegate ()
            {
                base.OnAppearing();

                if (!this.DisableAppearanceLifeCycle)
                {
                    this.BaseViewModel?.OnAppear();
                }
            });
        }
        protected override void OnDisappearing()
        {
            this.ExecuteMethod(nameof(OnDisappearing), delegate ()
            {
                base.OnDisappearing();

                if (!this.DisableAppearanceLifeCycle)
                {
                    this.BaseViewModel?.OnDisappear();
                }
            });
        }

        #endregion
    }
}
