using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Views
{
    public interface IDataViewAdjuster
    {
        Task AdjustItems(INestedDataViewModel viewModel, List<IDataViewItem> dataViewItems);
    }
}
