using Stencil.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stencil.Maui.Views.Standard.v1_0
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
            _dataViewComponents[SlimEditor.COMPONENT_NAME] = new SlimEditor();
            _dataViewComponents[FullEditor.COMPONENT_NAME] = new FullEditor();
            _dataViewComponents[SlimEntry.COMPONENT_NAME] = new SlimEntry();
            _dataViewComponents[FullEntry.COMPONENT_NAME] = new FullEntry();
            _dataViewComponents[Carousel.COMPONENT_NAME] = new Carousel();
            _dataViewComponents[Spacer.COMPONENT_NAME] = new Spacer();
            _dataViewComponents[GroupBegin.COMPONENT_NAME] = new GroupBegin();
            _dataViewComponents[GroupEnd.COMPONENT_NAME] = new GroupEnd();
            _dataViewComponents[Image.COMPONENT_NAME] = new Image();
            _dataViewComponents[DualColumnMarkdown.COMPONENT_NAME] = new DualColumnMarkdown();
            _dataViewComponents[HeaderTitleBar.COMPONENT_NAME] = new HeaderTitleBar();
            _dataViewComponents[Indicator.COMPONENT_NAME] = new Indicator();
            _dataViewComponents[SingleColumnView.COMPONENT_NAME] = new SingleColumnView();
            _dataViewComponents[DualColumnView.COMPONENT_NAME] = new DualColumnView();
            _dataViewComponents[TriColumnView.COMPONENT_NAME] = new TriColumnView();
            _dataViewComponents[DropDown.COMPONENT_NAME] = new DropDown();
            _dataViewComponents[GlyphHeader.COMPONENT_NAME] = new GlyphHeader();
            _dataViewComponents[CheckBox.COMPONENT_NAME] = new CheckBox();
            _dataViewComponents[ToggleView.COMPONENT_NAME] = new ToggleView();
            _dataViewComponents[CenterText.COMPONENT_NAME] = new CenterText();
            _dataViewComponents[TripleStackView.COMPONENT_NAME] = new TripleStackView();
            _dataViewComponents[ColumnCollection.COMPONENT_NAME] = new ColumnCollection();
            
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
