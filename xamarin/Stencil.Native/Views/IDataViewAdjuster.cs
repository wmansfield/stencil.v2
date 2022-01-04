using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Native.Views
{
    public interface IDataViewAdjuster
    {
        Task AdjustItems(IDataViewModel viewModel, List<IDataViewItem> dataViewItems);
    }
}
