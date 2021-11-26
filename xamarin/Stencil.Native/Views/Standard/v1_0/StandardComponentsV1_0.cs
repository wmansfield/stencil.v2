using System;
using System.Collections.Generic;

namespace Stencil.Native.Views.Standard.v1_0
{
    public class StandardComponentsV1_0 : IComponentLibrary
    {
        public StandardComponentsV1_0()
        {
            _dataViewComponents = new Dictionary<string, IDataViewComponent>(StringComparer.OrdinalIgnoreCase);
            _dataViewComponents[H1.COMPONENT_NAME] = new H1();
            _dataViewComponents[Button.COMPONENT_NAME] = new Button();
            _dataViewComponents[SimpleEntry.COMPONENT_NAME] = new SimpleEntry();
            _dataViewComponents[Carousel.COMPONENT_NAME] = new Carousel();
        }

        public const string NAME = "v1.0";

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
