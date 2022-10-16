using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Views
{
    public enum InteractionStateOperator
    {
        /// <summary>
        /// State can be any value
        /// </summary>
        any,
        /// <summary>
        /// State must equal provided value
        /// </summary>
        equals,
        /// <summary>
        /// State must not equal provided value
        /// </summary>
        not_equals,
    }
}
