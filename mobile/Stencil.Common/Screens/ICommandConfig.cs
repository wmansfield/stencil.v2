using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Common.Screens
{
    public interface ICommandConfig
    {
        string CommandName { get; }
        string CommandParameter { get; }
    }
}
