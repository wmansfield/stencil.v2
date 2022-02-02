using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Views.Standard
{
    public class InteractionStateMap : IInteractionStateMap
    {
        public InteractionStateOperator state_operator { get; set; }
        public string state { get; set; }
        public string value_format { get; set; }
    }
}
