using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui.Views
{
    public interface IDataViewAdjuster
    {
        Task AdjustItems(INestedDataViewModel viewModel, List<IDataViewItem> dataViewItems);
    }
}
