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
            _dataViewComponents[PrimaryButton.COMPONENT_NAME] = new PrimaryButton();
            _dataViewComponents[LightEntry.COMPONENT_NAME] = new LightEntry();
            _dataViewComponents[FullEntry.COMPONENT_NAME] = new FullEntry();
            _dataViewComponents[Carousel.COMPONENT_NAME] = new Carousel();
            _dataViewComponents[Spacer.COMPONENT_NAME] = new Spacer();
            _dataViewComponents[GroupBegin.COMPONENT_NAME] = new GroupBegin();
            _dataViewComponents[GroupEnd.COMPONENT_NAME] = new GroupEnd();
            _dataViewComponents[Image.COMPONENT_NAME] = new Image();
            _dataViewComponents[DualColumnMarkdown.COMPONENT_NAME] = new DualColumnMarkdown();
        }

        public const string NAME = "v1.0";

        private Dictionary<string, IDataViewComponent> _dataViewComponents;

        public IDataViewComponent GetComponent(string component)
        {
            if (_dataViewComponents.TryGetValue(component, out IDataViewComponent dataViewComponent))
            {
                return dataViewComponent;
            }
            //TODO:MUST: Meaningful reaction to this
            throw new Exception("Unable to find component with the name " + component);
        }
    }
}
