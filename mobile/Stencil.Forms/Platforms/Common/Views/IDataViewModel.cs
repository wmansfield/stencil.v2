using Stencil.Common.Screens;
using Stencil.Forms.Base;
using Stencil.Forms.Presentation.Menus;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Stencil.Forms.Views
{
    public interface IDataViewModel : INestedDataViewModel
    {
        ICommand NavigateBackCommand { get; }
        bool IsMenuSupported { get; }
        ObservableCollection<IMenuEntry> MenuEntries { get; }
        NavigationData NavigationData { get; set; }
    }
}
