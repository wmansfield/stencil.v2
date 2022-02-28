using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Views
{
    public interface IInteractionStateMap
    {
        InteractionStateOperator state_operator { get; }
        string state { get; }
        string value_format { get; }
    }
}
