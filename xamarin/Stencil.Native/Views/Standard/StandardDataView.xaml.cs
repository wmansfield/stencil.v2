using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Stencil.Native.Base;
using Stencil.Native.Presentation.Routing;
using Stencil.Native.Presentation.Menus;

namespace Stencil.Native.Views.Standard
{
    public partial class StandardDataView : BaseContentView, IRouterView, IDataViewVisual
    {
        public StandardDataView(IDataViewModel dataViewModel)
            : base(nameof(StandardDataView))
        {
            InitializeComponent();

            this.DataViewModel = dataViewModel;
            this.BindingContext = this.DataViewModel;
            this.DataViewModel.DataViewVisual = this;
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

        public Task OnNavigatingToAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnNavigatingToAsync), delegate ()
            {
                return this.DataViewModel.OnNavigatingToAsync();
            });
        }

        #endregion
    }
}