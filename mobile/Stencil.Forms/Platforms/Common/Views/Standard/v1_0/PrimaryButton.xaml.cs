using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Platform;
using Stencil.Forms.Resourcing;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class PrimaryButton : ResourceDictionary, IDataViewComponent
    {
        public PrimaryButton()
        {
            InitializeComponent();
        }



        public const string COMPONENT_NAME = "primaryButton";

        private const string TEMPLATE_KEY = "primaryButton";

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
                PrimaryButtonContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<PrimaryButtonContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new PrimaryButtonContext();
                }

                if (result.FontSize <= 0)
                {
                    result.FontSize = 16;
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.PrepareInteractions();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
        
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync($"{COMPONENT_NAME}.Button_Clicked", async delegate ()
            {
                View view = (sender as View);
                PrimaryButtonContext context = view?.BindingContext as PrimaryButtonContext;
                if (context != null)
                {
                    DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();

                    try
                    {
                        context.UIButtonBackgroundColor = AppColors.Primary400;
                        if (!string.IsNullOrWhiteSpace(context.CommandName))
                        {
                            if (context.CommandScope?.CommandProcessor != null)
                            {
                                await context.CommandScope.CommandProcessor.ExecuteCommandAsync(context.CommandScope, context.CommandName, context.CommandParameter, context?.DataViewItem?.DataViewModel);
                            }
                        }
                    }
                    finally
                    {
                        context.UIButtonBackgroundColor = AppColors.Primary900;
                    }
                }
            });
        }
    }

    public class PrimaryButtonContext : PreparedBindingContext, IStateResponder
    {
        public PrimaryButtonContext()
            : base(nameof(PrimaryButtonContext))
        {
        }
        public const string INTERACTION_KEY_TEXT = "text";
        public const string INTERACTION_KEY_HIDDEN = "hidden";

        public string CommandName { get; set; }
        public string CommandParameter { get; set; }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _uiButtonBackgroundColor = AppColors.Primary900;
        public string UIButtonBackgroundColor
        {
            get { return _uiButtonBackgroundColor; }
            set { SetProperty(ref _uiButtonBackgroundColor, value); }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private Thickness _padding = new Thickness(20, 0);
        public Thickness Padding
        {
            get { return _padding; }
            set { SetProperty(ref _padding, value); }
        }

        private Thickness _margin = new Thickness();
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        private bool _showIcon;
        public bool ShowIcon
        {
            get { return _showIcon; }
            set { SetProperty(ref _showIcon, value); }
        }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        private bool _hidden;
        public bool Hidden
        {
            get { return _hidden; }
            set
            {
                if (SetProperty(ref _hidden, value))
                {
                    this.RaisePropertyChanged(nameof(UIVisible));
                }
            }
        }

        public bool UIVisible
        {
            get
            {
                return !this.Hidden;
            }
        }

        private int _fontSize;
        public int FontSize
        {
            get { return _fontSize; }
            set { SetProperty(ref _fontSize, value); }
        }

        protected override void ApplyStateValue(string group, string state_key, string state, string value_key, string value)
        {
            base.ExecuteMethod(nameof(ApplyStateValue), delegate ()
            {
                switch (value_key)
                {
                    case INTERACTION_KEY_HIDDEN:
                        if (!string.IsNullOrEmpty(value))
                        {
                            this.Hidden = value.Equals("true", StringComparison.OrdinalIgnoreCase);
                        }
                        break;
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