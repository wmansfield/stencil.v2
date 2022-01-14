using System.Threading.Tasks;

namespace Stencil.Forms.Base
{
    public abstract class BaseViewModelView<TViewModel> : BaseContentView
        where TViewModel : BaseViewModel
    {
        public BaseViewModelView(string trackPrefix, TViewModel viewModel)
            : base(trackPrefix)
        {
            this.BaseViewModel = viewModel;
        }

        #region Properties

        public virtual BaseViewModel BaseViewModel { get; protected set; }

        public bool DisableAppearanceLifeCycle { get; set; }

        #endregion

        #region Lifecycle

        public virtual Task OnNavigatingToAsync()
        {
            return base.ExecuteFunction(nameof(OnNavigatingToAsync), delegate ()
            {
                if (!this.DisableAppearanceLifeCycle)
                {
                    if(this.BaseViewModel != null)
                    {
                        return this.BaseViewModel?.OnNavigatingToAsync();
                    }
                }
                return Task.CompletedTask;
            });
        }
        public virtual Task OnNavigatingFromAsync()
        {
            return base.ExecuteFunction(nameof(OnNavigatingFromAsync), delegate ()
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

        #endregion
    }
}
