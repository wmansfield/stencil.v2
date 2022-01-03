using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Resourcing;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class H3 : ResourceDictionary, IDataViewComponent
    {
        public H3()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "h3";

        private const string TEMPLATE_KEY = "h3";

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