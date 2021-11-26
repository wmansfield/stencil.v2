using System;
using System.Collections.Generic;

namespace Stencil.Native.Views.Standard.v1_1
{
    public class StandardComponentsV1_1 : IComponentLibrary
    {
        public StandardComponentsV1_1()
        {
            _dataViewComponents = new Dictionary<string, IDataViewComponent>(StringComparer.OrdinalIgnoreCase);
            _dataViewComponents[H1.COMPONENT_NAME] = new H1();
        }

        public const string NAME = "v1.1";

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
