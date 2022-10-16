using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class NamedValue : ResourceDictionary, IDataViewComponent
    {
        public NamedValue()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "namedValue";

        private const string TEMPLATE_KEY = "namedValue";

        private DataTemplate _dataTemplate;

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
                if (_dataTemplate == null)
                {
                    _dataTemplate = this[TEMPLATE_KEY] as DataTemplate;
                }
                return _dataTemplate;
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

                if (result.HeightRequest <= 0)
                {
                    result.HeightRequest = result.FontSize + 5;
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
            this.HeightRequest = -1;
        }

        private string _nameText;
        public string NameText
        {
            get { return _nameText; }
            set { SetProperty(ref _nameText, value); }
        }

        private string _nameTextColor = AppColors.PrimaryBlack;
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

        private string _valueTextColor = AppColors.PrimaryBlack;
        public string ValueTextColor
        {
            get { return _valueTextColor; }
            set { SetProperty(ref _valueTextColor, value); }
        }

        private string _backgroundColor = AppColors.Transparent;
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

        public int HeightRequest { get; set; }
    }
}