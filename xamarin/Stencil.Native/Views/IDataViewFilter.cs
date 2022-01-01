using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Native.Views
{
    public interface IDataViewFilter
    {
        Task<bool> ApplyFilter(IDataViewModel viewModel, IDataViewItem dataViewItem);
    }
}
