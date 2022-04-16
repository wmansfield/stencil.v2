using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Views
{
    public interface IResolvableTemplateSelector
    {
        IDataViewComponent ResolveTemplateAndPrepareData(IDataViewItem dataViewItem);
    }
}
