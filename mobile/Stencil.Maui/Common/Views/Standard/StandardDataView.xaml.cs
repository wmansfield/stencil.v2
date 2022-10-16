using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Stencil.Maui.Base;
using Stencil.Maui.Presentation.Routing;
using Stencil.Maui.Presentation.Menus;
using Microsoft.Maui.Controls;

namespace Stencil.Maui.Views.Standard
{
    public partial class StandardDataView : BaseContentView, IRouterView, IDataViewVisual
    {
        public StandardDataView(IDataViewModel dataViewModel)
            : base(nameof(StandardDataView))
        {
            InitializeComponent();

            dataViewModel.DataViewVisual = this;
            this.DataViewModel = dataViewModel;
            this.BindingContext = this.DataViewModel;
        }

        private IDataViewModel _viewModel;
        public IDataViewModel DataViewModel
        {
            get { return _viewModel; }
            set { SetProperty(ref _viewModel, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.ExecuteMethod(nameof(OnBindingContextChanged), delegate ()
            {
                base.OnBindingContextChanged();

                this.DataViewModel.DataViewVisual = this;

            });
        }


        #region IRouterView

        public ICommand NavigateBackCommand
        {
            get
            {
                return this.DataViewModel.NavigateBackCommand;
            }
        }
        public bool IsMenuSupported
        {
            get { return this.DataViewModel.IsMenuSupported; }
        }
        public ObservableCollection<IMenuEntry> MenuEntries
        {
            get { return this.DataViewModel.MenuEntries; }
        }
        public View GetSelf()
        {
            return this;
        }

        public Task OnNavigatingToAsync(bool reload)
        {
            return base.ExecuteMethodAsync(nameof(OnNavigatingToAsync), delegate ()
            {
                return this.DataViewModel.OnNavigatingToAsync(reload);
            });
        }
        public Task OnNavigatedToAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnNavigatedToAsync), delegate ()
            {
                return this.DataViewModel.OnNavigatedToAsync();
            });
        }
        #endregion
    }
}