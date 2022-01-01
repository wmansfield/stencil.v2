using Newtonsoft.Json;
using Stencil.Native.Commanding;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class Carousel : ResourceDictionary, IDataViewComponent
    {
        public Carousel()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "carousel";

        private const string TEMPLATE_KEY = "carousel";

        public bool PreparedDataCacheDisabled
        {
            get
            {
                return false;
            }
        }

        public DataTemplate GetDataTemplate()
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.GetDataTemplate", delegate ()
            {
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public object PrepareData(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.PrepareData", delegate ()
            {
                PreparedData result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<PreparedData>(configuration_json);
                }
                if(result == null)
                {
                    result = new PreparedData();
                }

                List<IDataViewModel> cells = new List<IDataViewModel>();
                
                if (dataViewItem?.Sections != null)
                {
                    foreach (IDataViewSection section in dataViewItem.Sections)
                    {
                        if (section.ViewItems != null)
                        {
                            StandardDataViewModel childViewModel = new StandardDataViewModel(commandScope.CommandProcessor, selector)
                            {
                                MainItemsFiltered = new ObservableCollection<IDataViewItem>(section.ViewItems)
                            };

                            cells.Add(childViewModel);
                        }
                    }
                }
                result.Cells = cells.ToArray();
                result.DataTemplateSelector = selector;
                return result;
            });
        }
        public class PreparedData
        {
            public string BackgroundColor { get; set; }

            public double HeightRequest { get; set; }

            [JsonIgnore]
            public DataTemplateSelector DataTemplateSelector { get; set; }

            [JsonIgnore]
            public IDataViewModel[] Cells { get; set; }
        }
    }
}