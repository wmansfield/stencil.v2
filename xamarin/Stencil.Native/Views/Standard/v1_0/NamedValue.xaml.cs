using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Resourcing;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class NamedValue : ResourceDictionary, IDataViewComponent
    {
        public NamedValue()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "namedValue";

        private const string TEMPLATE_KEY = "namedValue";

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
                if(result.FontSize <= 0)
                {
                    result.FontSize = 16;
                }
                return result;
            });
        }
        public class PreparedData : PropertyClass
        {
            private string _nameText;
            public string NameText
            {
                get { return _nameText; }
                set { SetProperty(ref _nameText, value); }
            }

            private string _nameTextColor;
            public string NameTextColor
            {
                get { return _nameTextColor; }
                set { SetProperty(ref _nameTextColor, value); }
            }

            private string _valueText;
            public string ValueText
            {
                get { return _valueText; }
                set { SetProperty(ref _valueText, value); }
            }

            private string _valueTextColor;
            public string ValueTextColor
            {
                get { return _valueTextColor; }
                set { SetProperty(ref _valueTextColor, value); }
            }

            private string _backgroundColor;
            public string BackgroundColor
            {
                get { return _backgroundColor; }
                set { SetProperty(ref _backgroundColor, value); }
            }

            private int _fontSize;
            public int FontSize
            {
                get { return _fontSize; }
                set { SetProperty(ref _fontSize, value); }
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