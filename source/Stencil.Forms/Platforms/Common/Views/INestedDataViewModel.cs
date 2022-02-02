using Stencil.Forms.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views
{
    public interface INestedDataViewModel : IBaseViewModel
    {
        IDataViewVisual DataViewVisual { get; set; }
        ObservableCollection<IDataViewItem> MainItemsUnFiltered { get; }
        ObservableCollection<object> MainItemsFiltered { get; }
        ObservableCollection<object> FooterItems { get; }
        ObservableCollection<object> HeaderItems { get; }
        bool ShowFooter { get; }
        DataTemplateSelector DataTemplateSelector { get; }
        Thickness Padding { get; }
        Color BackgroundColor { get; }
        string BackgroundImage { get; }

        List<IStateEmitter> StateEmitters { get; }
        List<IDataViewFilter> Filters { get; }
        List<IDataViewAdjuster> Adjusters { get; }
        Dictionary<string, List<IStateResponder>> StateResponders { get; }
        void AddStateResponder(IStateResponder stateResponder);
        void RaiseStateChange(string group, string name, string state);

        Task Initialize();
        Task ApplyFiltersAndAdjustmentsAsync();
        Task ExtractAndPrepareExtensionsAsync();
        Task ExtractAndPrepareExtensionsAsync(IDataViewItem item);
    }
}
