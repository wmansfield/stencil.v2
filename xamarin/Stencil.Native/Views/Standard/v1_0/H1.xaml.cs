using Newtonsoft.Json;
using Stencil.Native.Commanding;

using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class H1 : ResourceDictionary, IDataViewComponent
    {
        public H1()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "h1-" + StandardComponentsV1_0.NAME;

        private const string TEMPLATE_KEY = "h1";

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
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    return JsonConvert.DeserializeObject<PreparedData>(configuration_json);
                }
                return new PreparedData();
            });
        }
        public class PreparedData
        {
            public string Text { get; set; }
            public string FormattedText { get; set; }
        }
    }
}