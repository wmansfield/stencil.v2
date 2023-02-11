using Starter.App.Views.V1;
using Stencil.Maui.Commanding;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Views
{
    public class StarterComponentLibraryTemplateSelector : ComponentLibraryTemplateSelector
    {
        static StarterComponentLibraryTemplateSelector()
        {
            List<IComponentLibrary> customLibrary = new List<IComponentLibrary>();
            customLibrary.Add(new StarterComponentsV1()); // order matters, first match wins

            ComponentLibraryTemplateSelector.ComponentLibraries[StarterLibrary.LIBRARY_NAME] = customLibrary;
        }

        public StarterComponentLibraryTemplateSelector(ICommandScope scope)
            : base(scope)
        {

        }
    }
}
