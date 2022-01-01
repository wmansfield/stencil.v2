using Stencil.Native.Commanding;
using Xamarin.Forms;

namespace Stencil.Native.Views
{
    public interface IDataViewComponent
    {
        bool PreparedDataCacheDisabled { get; }
        DataTemplate GetDataTemplate();
        object PrepareData(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json);
    }
}
