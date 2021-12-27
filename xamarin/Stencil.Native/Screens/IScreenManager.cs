using Stencil.Native.Commanding;
using Stencil.Native.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stencil.Native.Screens
{
    public interface IScreenManager
    {
        Task<IDataViewModel> GenerateViewModelAsync(ICommandProcessor commandProcessor, INavigationData navigationData);
        IDataViewItem GenerateViewItem(IViewConfig viewConfig);

        Task<ScreenConfig> RetrieveScreenConfigAsync(string screenStorageKey, bool includeExpired);
        Task SaveScreenConfigAsync(ScreenConfig screenConfig);
        Task InvalidateScreenConfigAsync(string screenStorageKey);

        Task<List<ScreenConfig>> GetScreenConfigsWithNameAsync(string screenName);
        List<IScreenConfig> GetForDownloading();

    }
}
