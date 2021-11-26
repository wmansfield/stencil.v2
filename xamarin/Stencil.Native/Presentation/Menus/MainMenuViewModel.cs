using Stencil.Native.Base;
using Stencil.Native.Commanding;
using System.Collections.Generic;

namespace Stencil.Native.Presentation.Menus
{
    public class MainMenuViewModel : BaseViewModel, IMenuViewModel
    {
        public MainMenuViewModel()
            : base(nameof(MainMenuViewModel))
        {
            this.MenuEntries = new List<IMenuEntry>();
        }
        public IList<IMenuEntry> MenuEntries { get; set; }

        public ICommandProcessor CommandProcessor { get; set; }

    }
}
