using Stencil.Native.Commanding;
using Xamarin.Forms;

namespace Stencil.Native.Views
{
    public interface IDataViewComponent
    {
        DataTemplate GetDataTemplate();
        object PrepareData(ICommandScope commandScope, DataTemplateSelector selector, string configuration_json, IDataViewSection[] sections);
    }
}
