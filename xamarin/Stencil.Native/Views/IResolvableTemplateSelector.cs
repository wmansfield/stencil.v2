using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Views
{
    public interface IResolvableTemplateSelector
    {
        IDataViewComponent ResolveTemplateAndPrepareData(IDataViewItem dataViewItem);
    }
}
