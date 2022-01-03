using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Resourcing;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class H2 : ResourceDictionary, IDataViewComponent
    {
        public H2()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "h2";

        private const string TEMPLATE_KEY = "h2";

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
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    return JsonConvert.DeserializeObject<PreparedData>(configuration_json);
                }
                return new PreparedData();
            });
        }
        public class PreparedData : PropertyClass
        {
            private string _text;
            public string Text
            {
                get { return _text; }
                set { SetProperty(ref _text, value); }
            }

            private string _textColor;
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