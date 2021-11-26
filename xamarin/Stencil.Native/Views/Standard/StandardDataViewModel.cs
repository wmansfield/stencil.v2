using Stencil.Native.Base;
using Stencil.Native.Commanding;
using Stencil.Native.Presentation.Menus;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard
{
    public class StandardDataViewModel : BaseViewModel, IDataViewModel
    {
        public StandardDataViewModel(ICommandProcessor commandProcessor, DataTemplateSelector dataTemplateSelector = null)
            : base(nameof(StandardDataViewModel))
        {
            this.CommandScope = new CommandScope(commandProcessor);
            this.DataTemplateSelector = dataTemplateSelector;
            if (this.DataTemplateSelector == null)
            {
                this.DataTemplateSelector = new ComponentLibraryTemplateSelector(this.CommandScope);
            }
        }

        public ICommandScope CommandScope { get; set; }
        public IDataViewVisual DataViewVisual { get; set; }
        public bool IsMenuSupported { get; set; }
        public DataTemplateSelector DataTemplateSelector { get; set; }


        private ObservableCollection<IDataViewItem> _dataViewItems;
        public ObservableCollection<IDataViewItem> DataViewItems
        {
            get { return _dataViewItems; }
            set { SetProperty(ref _dataViewItems, value); }
        }


        private ObservableCollection<IMenuEntry> _menuEntries;
        public ObservableCollection<IMenuEntry> MenuEntries
        {
            get { return _menuEntries; }
            set { SetProperty(ref _menuEntries, value); }
        }

        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }
    }
}
