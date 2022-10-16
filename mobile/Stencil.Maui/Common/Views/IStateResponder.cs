using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Views
{
    public interface IStateResponder
    {
        string[] GetInteractionGroups();
        void OnInteractionStateChanged(string group, string name, string state);
    }
}
