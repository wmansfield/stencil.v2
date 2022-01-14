using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Resourcing;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class HeaderWithIcon : ResourceDictionary, IDataViewComponent
    {
        public HeaderWithIcon()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "headerWithIcon";

        private const string TEMPLATE_KEY = "headerWithIcon";

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
                HeaderWithIconContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<HeaderWithIconContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new HeaderWithIconContext();
                }
                if(result.Fontsize <= 0)
                {
                    result.Fontsize = 16;
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return result;
            });
        }
    }

    public class HeaderWithIconContext : PreparedBingingContext
    {
        public HeaderWithIconContext()
            : base(nameof(HeaderWithIconContext))
        {

        }

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

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        private bool _showIcon;
        public bool ShowIcon
        {
            get { return _showIcon; }
            set { SetProperty(ref _showIcon, value); }
        }

        private Thickness _padding;
        public Thickness Padding
        {
            get { return _padding; }
            set { SetProperty(ref _padding, value); }
        }
    }
}