using System.Threading.Tasks;

namespace Stencil.Forms.Base
{
    public interface IBaseViewModel
    {
        #region Lifecycle Methods

        Task OnNavigatingToAsync(bool reload);
        Task OnNavigatedToAsync();

        Task OnNavigatingFromAsync();

        void OnAppear();
        void OnDisappear();

        #endregion
    }
}
