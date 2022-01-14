using System.Threading.Tasks;

namespace Stencil.Forms.Base
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
