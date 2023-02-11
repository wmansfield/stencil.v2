using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class H2 : ResourceDictionary, IDataViewComponent
    {
        public H2()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "h2";
        public const string INTERACTION_KEY_TEXT = "text";
        public const string INTERACTION_KEY_VISIBLE = "visible";

        private const string TEMPLATE_KEY = "h2";

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
        public Task<IDataViewItemReference> PrepareBindingContextAsync(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(PrepareBindingContextAsync)}", delegate ()
            {
                H2Context result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<H2Context>(configuration_json);
                }

                if (result == null)
                {
                    result = new H2Context();
                }
                if (result.FontSize <= 0)
                {
                    try
                    {
                        result.FontSize = (int)Application.Current.Resources["FontSizeH2"];
                    }
                    catch
                    {
                        result.FontSize = 20;
                    }
                }
                if (string.IsNullOrWhiteSpace(result.FontFamily))
                {
                    result.FontFamily = "SansBold";
                }
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.EnsureInteractionsPrepared();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class H2Context : PreparedBindingContext, IStateResponder
    {
        public H2Context()
            : base(nameof(H2Context))
        {

        }

        private string _fontFamily = "SansBold";
        public string FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set { SetProperty(ref _fontFamily, value); }
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

        protected override void ApplyStateValue(string group, string state_key, string state, string value_key, string value)
        {
            base.ExecuteMethod(nameof(ApplyStateValue), delegate ()
            {
                switch (value_key)
                {
                    case H2.INTERACTION_KEY_TEXT:
                    default:
                        this.Text = value;
                        break;
                }
            });
        }
    }
}