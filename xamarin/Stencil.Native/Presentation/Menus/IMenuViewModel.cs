using Stencil.Native.Commanding;
using System.Collections.Generic;

namespace Stencil.Native.Presentation.Menus
{
    public interface IMenuViewModel
    {
        IList<IMenuEntry> MenuEntries { get; }
        ICommandProcessor CommandProcessor { get; set; }
    }
}
