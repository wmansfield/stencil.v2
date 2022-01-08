using Newtonsoft.Json;
using Stencil.Native.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class Spacer : ResourceDictionary, IDataViewComponent
    {
        public Spacer()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "spacer";

        private const string TEMPLATE_KEY = "spacer";

        public bool BindingContextCacheEnabled
        {
            get
            {
                return true;
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
                SpacerContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<SpacerContext>(configuration_json);
                }

                if(result == null)
                {
                    result = new SpacerContext();
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return result;
            });
        }
    }

    public class SpacerContext : PreparedBingingContext
    {
        public SpacerContext()
            : base(nameof(SpacerContext))
        {

        }

        public int Height { get; set; }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }
    }
}