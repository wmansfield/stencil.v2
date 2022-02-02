using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Resourcing;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class NamedValue : ResourceDictionary, IDataViewComponent
    {
        public NamedValue()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "namedValue";

        private const string TEMPLATE_KEY = "namedValue";

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
                NamedValueContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<NamedValueContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new NamedValueContext();
                }
                if(result.FontSize <= 0)
                {
                    result.FontSize = 16;
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class NamedValueContext : PreparedBindingContext
    {
        public NamedValueContext()
            : base(nameof(NamedValueContext))
        {

        }

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