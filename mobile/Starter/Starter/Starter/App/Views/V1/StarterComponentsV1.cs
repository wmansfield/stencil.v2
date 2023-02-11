using Stencil.Common;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Views.V1
{
    public class StarterComponentsV1 : IComponentLibrary
    {
        public StarterComponentsV1()
        {
            _dataViewComponents = new Dictionary<string, IDataViewComponent>(StringComparer.OrdinalIgnoreCase);
            _dataViewComponents[SampleView.COMPONENT_NAME] = new SampleView();


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
