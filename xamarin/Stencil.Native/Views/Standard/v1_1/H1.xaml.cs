using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Resourcing;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_1
{
    /// <summary>
    /// Demonstrative only -- same as version 1.0  :)
    /// </summary>
    public partial class H1 : ResourceDictionary, IDataViewComponent
    {
        public H1()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "h1";

        private const string TEMPLATE_KEY = "h1";

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
                H1Context result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<H1Context>(configuration_json);
                }

                if (result == null)
                {
                    result = new H1Context();
                }
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return result;
            });
        }
    }

    public class H1Context : PreparedBingingContext
    {
        public H1Context()
            : base(nameof(H1Context))
        {

        }

        public string Text { get; set; }

        private string _textColor = AppColors.TextOverBackground;
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