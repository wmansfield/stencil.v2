using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Resourcing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
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

                result.PrepareInteractions();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class PlainTextContext : PreparedBindingContext, IStateResponder
    {
        public PlainTextContext()
            : base(nameof(PlainTextContext))
        {

        }

        public const string INTERACTION_KEY_TEXT = "text";

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

        private string _textColor = AppColors.PrimaryBlack;
        public string TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

        private string _backgroundColor = AppColors.Transparent;
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

        private bool _center;
        public bool Center
        {
            get { return _center; }
            set
            {
                if (SetProperty(ref _center, value))
                {
                    this.OnPropertyChanged(nameof(UITextAlignment));
                }
            }
        }

        public TextAlignment UITextAlignment
        {
            get
            {
                if(this.Center)
                {
                    return TextAlignment.Center;
                }
                else
                {
                    return TextAlignment.Start;
                }
            }
        }
        

        protected override void ApplyStateValue(string group, string state_key, string state, string value_key, string value)
        {
            base.ExecuteMethod(nameof(ApplyStateValue), delegate ()
            {
                switch (value_key)
                {
                    case INTERACTION_KEY_TEXT:
                        this.Text = value;
                        break;
                    default:
                        break;
                }
            });
        }
    }
}