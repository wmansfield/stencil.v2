
using System.Collections.Generic;

namespace Stencil.Forms.Presentation.Menus
{
    public interface IMenuEntry
    {
        string Identifier { get; }
        bool IsIcon { get; }
        string IconCharacter { get; }
        string Label { get; }
        string CommandName { get; }
        string CommandParameter { get; }

        string ActiveBackgroundColor { get; }
        string ActiveTextColor { get; }

        string SelectedBackgroundColor { get; }
        string SelectedTextColor { get; }

        string UnselectedBackgroundColor { get; }
        string UnselectedTextColor { get; }


        string UIBackgroundColor { get; }
        string UITextColor { get; }
        bool UISelected { get; set; }
        bool UIActive { get; set; }

    }
}
