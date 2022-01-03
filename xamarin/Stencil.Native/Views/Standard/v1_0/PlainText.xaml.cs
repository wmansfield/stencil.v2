using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Resourcing;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class PlainText : ResourceDictionary, IDataViewComponent
    {
        public PlainText()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "plainText";

        private const string TEMPLATE_KEY = "plainText";

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
                    return JsonConvert.DeserializeObject<PreparedData>(configuration_json);
                }
                if(result == null)
                {
                    result = new PreparedData();
                }
                if(result.Fontsize <= 0)
                {
                    result.Fontsize = 16;
                }
                return result;
            });
        }
        public class PreparedData : PropertyClass
        {
            private int _fontSize;
            public int Fontsize
            {
                get { return _fontSize; }
                set { SetProperty(ref _fontSize, value); }
            }

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

            private Thickness _padding;
            public Thickness Padding
            {
                get { return _padding; }
                set { SetProperty(ref _padding, value); }
            }
        }
    }
}