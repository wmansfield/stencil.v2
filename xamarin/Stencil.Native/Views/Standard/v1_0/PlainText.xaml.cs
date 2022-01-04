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
                PlainTextContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<PlainTextContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new PlainTextContext();
                }
                if(result.FontSize <= 0)
                {
                    result.FontSize = 16;
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return result;
            });
        }
    }

    public class PlainTextContext : PreparedBingingContext
    {
        public PlainTextContext()
            : base(nameof(PlainTextContext))
        {

        }

        private int _fontSize;
        public int FontSize
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