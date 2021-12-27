using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Views.Markdown
{
    public class MarkdownComponentsV1_0 : IComponentLibrary
    {
        public MarkdownComponentsV1_0()
        {
            _dataViewComponents = new Dictionary<string, IDataViewComponent>(StringComparer.OrdinalIgnoreCase);
            _dataViewComponents[MarkdownContainer.COMPONENT_NAME] = new MarkdownContainer();
        }

        private Dictionary<string, IDataViewComponent> _dataViewComponents;

        public IDataViewComponent GetComponent(string component)
        {
            if (_dataViewComponents.TryGetValue(component, out IDataViewComponent dataViewComponent))
            {
                return dataViewComponent;
            }
            return null;
        }
    }
}
