﻿
namespace Stencil.Native.Views.Standard
{
    public class StandardDataViewItem : PropertyClass, IDataViewItem
    {
        public StandardDataViewItem()
        {

        }
        public string Library { get; set; }
        public string Component { get; set; }

        private object _preparedData;
        public object PreparedData
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
    }
}