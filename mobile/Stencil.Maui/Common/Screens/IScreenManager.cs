using Stencil.Common.Screens;
using Stencil.Maui.Commanding;
using Stencil.Maui.Views;
using Stencil.Maui.Views.Standard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stencil.Maui.Screens
{
    public interface IScreenManager
    {
        Task<IDataViewModel> GenerateViewModelAsync(ICommandProcessor commandProcessor, INavigationData navigationData);
        /// <summary>
        /// Not intended to be used by your code.
        /// </summary>
        Task PrepareViewModelAsync(ICommandProcessor commandProcessor, IScreenConfig screenConfig, StandardDataViewModel viewModel);
        IDataViewItem GenerateViewItem(IDataViewModel dataViewModel, IViewConfig viewConfig);

        Task<ScreenConfig> RetrieveScreenConfigAsync(string screenStorageKey, bool includeExpired);
        Task SaveScreenConfigAsync(ScreenConfig screenConfig);
        Task RemoveScreenConfigAsync(string screenStorageKey);
        Task InvalidateScreenConfigAsync(string screenStorageKey);

        Task<List<ScreenConfig>> GetScreenConfigsWithNameAsync(string screenName);
        List<IScreenConfig> GetForDownloading();

        Task<IScreenConfig> LoadScreenConfigAsync(ICommandProcessor commandProcessor, INavigationData navigationData);

    }
}
