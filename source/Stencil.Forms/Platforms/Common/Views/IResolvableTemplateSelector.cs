using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Views
{
    public interface IResolvableTemplateSelector
    {
        Task<IDataViewComponent> ResolveTemplateAndPrepareDataAsync(IDataViewItem dataViewItem);
    }
}
