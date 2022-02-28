using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class HeaderTitleBar : ResourceDictionary, IDataViewComponent
    {
        public HeaderTitleBar()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "headerTitleBar";

        private const string TEMPLATE_KEY = "headerTitleBar";

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
                HeaderTitleBarContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<HeaderTitleBarContext>(configuration_json);
                }
                if (string.IsNullOrWhiteSpace(result.CommandName))
                {
                    result.CommandName = "app.navigate.pop";//TODO:MUST:Magic String
                    result.CommandParameter = null;
                }
                if(string.IsNullOrWhiteSpace(result.TextColor))
                {
                    result.TextColor = AppColors.TextOverPrimary;
                }
                if (string.IsNullOrWhiteSpace(result.BackIcon))
                {
                    result.BackIcon = "";
                }
                
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;
                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync(nameof(TapGestureRecognizer_Tapped), async delegate ()
            {
                View view = (sender as View);
                HeaderTitleBarContext context = view?.BindingContext as HeaderTitleBarContext;
                if (context != null)
                {
                    if (context.CommandScope?.CommandProcessor != null)
                    {
                        await context.CommandScope.CommandProcessor.ExecuteCommandAsync(context.CommandScope, context.CommandName, context.CommandParameter, context?.DataViewItem?.DataViewModel);
                    }
                }
            });
        }
    }

    public class HeaderTitleBarContext : PreparedBindingContext
    {
        public HeaderTitleBarContext()
            : base(nameof(HeaderTitleBarContext))
        {
            this.IconFontSize = 24;
            this.TitleFontSize = 16;
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

        private string _backIcon;
        public string BackIcon
        {
            get { return _backIcon; }
            set { SetProperty(ref _backIcon, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private double _iconFontSize;
        public double IconFontSize
        {
            get { return _iconFontSize; }
            set { SetProperty(ref _iconFontSize, value); }
        }

        private double _titleFontSize;
        public double TitleFontSize
        {
            get { return _titleFontSize; }
            set { SetProperty(ref _titleFontSize, value); }
        }

        public string CommandName { get; set; }
        public string CommandParameter { get; set; }

    }
}