using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Views
{
    public interface IResolvableTemplateSelector
    {
        IDataViewComponent ResolveTemplateAndPrepareData(IDataViewItem dataViewItem);
    }
}
