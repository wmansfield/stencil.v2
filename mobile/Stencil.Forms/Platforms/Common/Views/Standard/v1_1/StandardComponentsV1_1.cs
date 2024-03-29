﻿using Stencil.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stencil.Forms.Views.Standard.v1_1
{
    public class StandardComponentsV1_1 : IComponentLibrary
    {
        public StandardComponentsV1_1()
        {
            _dataViewComponents = new Dictionary<string, IDataViewComponent>(StringComparer.OrdinalIgnoreCase);

            foreach (string key in _dataViewComponents.Keys.ToList())
            {
                _dataViewComponents[ComponentUtility.GenerateVersionedName(key, VERSION)] = _dataViewComponents[key];
            }
        }

        public const string VERSION = "1.1";

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
