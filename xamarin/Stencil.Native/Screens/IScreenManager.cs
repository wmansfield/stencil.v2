using Stencil.Native.Commanding;
using Stencil.Native.Views;
using System.Threading.Tasks;

namespace Stencil.Native.Screens
{
    public interface IScreenManager
    {
        Task<IDataViewModel> GenerateScreenAsync(ICommandProcessor commandProcessor, string screenName);
        IDataViewItem GenerateViewItem(IViewConfig viewConfig);
    }
}
