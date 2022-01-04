using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Native.Views
{
    public interface IDataViewFilter
    {
        Task<bool> ShouldSuppressItem(IDataViewModel viewModel, IDataViewItem dataViewItem, List<IDataViewItem> dataSoFar);
    }
}
