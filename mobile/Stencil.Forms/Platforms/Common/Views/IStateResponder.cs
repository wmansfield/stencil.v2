using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Views
{
    public interface IStateResponder
    {
        string[] GetInteractionGroups();
        void OnInteractionStateChanged(string group, string name, string state);
    }
}
