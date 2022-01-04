using Newtonsoft.Json;
using Stencil.Native.Commanding;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public bool BindingContextCacheEnabled
        {
            get
            {
                return false;
            }
        }

        public DataTemplate GetDataTemplate()
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(GetDataTemplate)}", delegate ()
            {
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public IDataViewItemReference PrepareBindingContext(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(PrepareBindingContext)}", delegate ()
            {
                CarouselContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<CarouselContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new CarouselContext();
                }

                List<IDataViewModel> cells = new List<IDataViewModel>();
                
                if (dataViewItem?.Sections != null)
                {
                    foreach (IDataViewSection section in dataViewItem.Sections)
                    {
                        if (section.ViewItems?.Length > 0)
                        {
                            StandardDataViewModel childViewModel = new StandardDataViewModel(commandScope.CommandProcessor, selector)
                            {
                                MainItemsUnFiltered = new ObservableCollection<IDataViewItem>(section.ViewItems),
                                MainItemsFiltered = new ObservableCollection<object>(section.ViewItems.Select(x => x.PreparedContext))
                            };

                            cells.Add(childViewModel);
                        }
                    }
                }
                result.Cells = cells.ToArray();
                result.DataTemplateSelector = selector;

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;
                return result;
            });
        }
    }

    public class CarouselContext : PreparedBingingContext
    {
        public CarouselContext()
            : base(nameof(CarouselContext))
        {

        }
        public string BackgroundColor { get; set; }

        public double HeightRequest { get; set; }

        [JsonIgnore]
        public DataTemplateSelector DataTemplateSelector { get; set; }

        [JsonIgnore]
        public IDataViewModel[] Cells { get; set; }
    }
}