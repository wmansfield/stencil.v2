using Stencil.Maui.Commanding;
using System.Collections.Generic;

namespace Stencil.Maui.Presentation.Menus
{
    public interface IMenuViewModel
    {
        string SelectedIdentifier { get; set; }
        IList<IMenuEntry> MenuEntries { get; }
        IList<IMenuEntry> MenuEntriesFiltered { get; }
        ICommandProcessor CommandProcessor { get; set; }
    }
}
