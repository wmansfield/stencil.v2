using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Views
{
    public interface IDataViewFilter
    {
        Task<bool> ShouldSuppressItem(INestedDataViewModel viewModel, IDataViewItem dataViewItem, List<IDataViewItem> dataSoFar);
    }
}
