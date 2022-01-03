using System;
using System.Collections.Generic;
using System.Linq;

namespace Stencil.Native.Views.Standard.v1_0
{
    public class StandardComponentsV1_0 : IComponentLibrary
    {
        public StandardComponentsV1_0()
        {
            _dataViewComponents = new Dictionary<string, IDataViewComponent>(StringComparer.OrdinalIgnoreCase);
            _dataViewComponents[PlainText.COMPONENT_NAME] = new PlainText();
            _dataViewComponents[H1.COMPONENT_NAME] = new H1();
            _dataViewComponents[H2.COMPONENT_NAME] = new H2();
            _dataViewComponents[H3.COMPONENT_NAME] = new H3();
            _dataViewComponents[HeaderWithIcon.COMPONENT_NAME] = new HeaderWithIcon();
            _dataViewComponents[NamedValue.COMPONENT_NAME] = new NamedValue();
            _dataViewComponents[PrimaryButton.COMPONENT_NAME] = new PrimaryButton();
            _dataViewComponents[SlimEntry.COMPONENT_NAME] = new SlimEntry();
            _dataViewComponents[FullEntry.COMPONENT_NAME] = new FullEntry();
            _dataViewComponents[Carousel.COMPONENT_NAME] = new Carousel();
            _dataViewComponents[Spacer.COMPONENT_NAME] = new Spacer();
            _dataViewComponents[GroupBegin.COMPONENT_NAME] = new GroupBegin();
            _dataViewComponents[GroupEnd.COMPONENT_NAME] = new GroupEnd();
            _dataViewComponents[Image.COMPONENT_NAME] = new Image();
            _dataViewComponents[DualColumnMarkdown.COMPONENT_NAME] = new DualColumnMarkdown();
            _dataViewComponents[HeaderBackBar.COMPONENT_NAME] = new HeaderBackBar();
            
            foreach (string key in _dataViewComponents.Keys.ToList())
            {
                _dataViewComponents[ComponentUtility.GenerateVersionedName(key, VERSION)] = _dataViewComponents[key];
            }
        }

        public const string VERSION = "1.0";

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
