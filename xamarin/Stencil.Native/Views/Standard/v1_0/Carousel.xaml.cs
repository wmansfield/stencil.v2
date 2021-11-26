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

        public const string COMPONENT_NAME = "carousel-" + StandardComponentsV1_0.NAME;

        private const string TEMPLATE_KEY = "carousel";

        public DataTemplate GetDataTemplate()
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.GetDataTemplate", delegate ()
            {
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public object PrepareData(ICommandScope commandScope, DataTemplateSelector selector, string configuration_json, IDataViewSection[] sections)
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
                
                if (sections != null)
                {
                    foreach (IDataViewSection section in sections)
                    {
                        if (section.ViewItems != null)
                        {
                            StandardDataViewModel dataViewModel = new StandardDataViewModel(commandScope.CommandProcessor, selector)
                            {
                                DataViewItems = new ObservableCollection<IDataViewItem>(section.ViewItems)
                            };

                            cells.Add(dataViewModel);
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