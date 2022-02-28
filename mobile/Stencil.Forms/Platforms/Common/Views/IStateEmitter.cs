using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Views
{
    public interface IStateEmitter
    {
        string InteractionGroup { get; }
        void EmitDefaultState(INestedDataViewModel viewModel);
        Task ChangeStateAsync(string name, string state);
    }
}
