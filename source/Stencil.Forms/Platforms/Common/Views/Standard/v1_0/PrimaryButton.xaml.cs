using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Platform;
using Stencil.Forms.Resourcing;
using System;
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
        public IDataViewItemReference PrepareBindingContext(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(PrepareBindingContext)}", delegate ()
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

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return result;
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
                                await context.CommandScope.CommandProcessor.ExecuteCommandAsync(context.CommandScope, context.CommandName, context.CommandParameter);
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

    public class PrimaryButtonContext : PreparedBingingContext
    {
        public PrimaryButtonContext()
            : base(nameof(PrimaryButtonContext))
        {

        }

        public string Text { get; set; }
        public string CommandName { get; set; }
        public string CommandParameter { get; set; }

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
    }
}