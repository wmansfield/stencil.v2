using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Resourcing;
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
        public class PreparedData : PropertyClass
        {
            public string Text { get; set; }

            private string _textColor = AppColors.TextOverBackground;
            public string TextColor
            {
                get { return _textColor; }
                set { SetProperty(ref _textColor, value); }
            }

            private string _backgroundColor;
            public string BackgroundColor
            {
                get { return _backgroundColor; }
                set { SetProperty(ref _backgroundColor, value); }
            }
        }
    }
}