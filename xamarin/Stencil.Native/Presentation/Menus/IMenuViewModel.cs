using Stencil.Native.Commanding;
using System.Collections.Generic;

namespace Stencil.Native.Presentation.Menus
{
    public interface IMenuViewModel
    {
        string SelectedIdentifier { get; set; }
        IList<IMenuEntry> MenuEntries { get; }
        ICommandProcessor CommandProcessor { get; set; }
    }
}
