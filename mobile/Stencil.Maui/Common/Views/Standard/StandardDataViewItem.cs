
using Microsoft.Maui.Controls;
using System;

namespace Stencil.Maui.Views.Standard
{
    public class StandardDataViewItem : PropertyClass, IDataViewItem
    {
        public StandardDataViewItem()
        {

        }
        public string Library { get; set; }
        public string Component { get; set; }


        private object _preparedData;
        public object PreparedContext
        {
            get { return _preparedData; }
            set { SetProperty(ref _preparedData, value); }
        }

        private string _configurationJson;
        public string ConfigurationJson
        {
            get { return _configurationJson; }
            set { SetProperty(ref _configurationJson, value); }
        }
        private IDataViewSection[] _sections;
        public IDataViewSection[] Sections
        {
            get { return _sections; }
            set { SetProperty(ref _sections, value); }
        }


        /// <summary>
        /// Warning: Does not support property changed notification, rely on custom application of changes instead
        /// </summary>
        public IDataViewItem[] EncapsulatedItems { get; set; }

        /// <summary>
        /// Warning: Does not support property changed notification, rely on custom application of changes instead
        /// </summary>
        public IDataViewFilter ViewFilter { get; set; }


        /// <summary>
        /// Warning: Does not support property changed notification, rely on custom application of changes instead
        /// </summary>
        public IDataViewModel DataViewModel { get; set; }


        public WeakReference<DataTemplate> UIDataTemplate { get; set; }



    }
}
