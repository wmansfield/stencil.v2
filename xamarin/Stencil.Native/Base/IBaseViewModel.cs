using System.Threading.Tasks;

namespace Stencil.Native.Base
{
    public interface IBaseViewModel
    {
        #region Lifecycle Methods

        Task OnNavigatingToAsync();
        Task OnNavigatingFromAsync();

        void OnAppear();
        void OnDisappear();

        #endregion
    }
}
