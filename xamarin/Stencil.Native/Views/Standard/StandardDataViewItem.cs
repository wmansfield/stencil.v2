
namespace Stencil.Native.Views.Standard
{
    public class StandardDataViewItem : StandardDataViewItem<object>
    {

    }
    public class StandardDataViewItem<TPrepareData> : PropertyClass, IDataViewItem
        where TPrepareData : class
    {
        public StandardDataViewItem()
        {

        }
        public string Library { get; set; }
        public string Component { get; set; }

        private TPrepareData _preparedData;
        public TPrepareData PreparedData
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
        /// Warning: Does not support change notification, rely on custom application of changes instead
        /// </summary>
        public IDataViewFilter ViewFilter { get; set; }


        /// <summary>
        /// Warning: Does not support change notification, rely on custom application of changes instead
        /// </summary>
        public IDataViewModel DataViewModel { get; set; }
        object IDataViewItem.PreparedData
        {
            get
            {
                return this.PreparedData;
            }
            set
            {
                this.PreparedData = value as TPrepareData;
            }
        }
    }
}
