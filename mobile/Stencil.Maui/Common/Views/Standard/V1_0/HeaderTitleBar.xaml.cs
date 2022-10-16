using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Stencil.Maui.Views.Standard.v1_0
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
                if (string.IsNullOrWhiteSpace(result.LeftCommandName))
                {
                    result.LeftCommandName = "app.navigate.pop";//TODO:MUST:Magic String
                    result.LeftCommandParameter = null;
                }
                if (string.IsNullOrWhiteSpace(result.LeftIcon))
                {
                    result.LeftIcon = ""; // convention
                }
                if (string.IsNullOrWhiteSpace(result.TextColor))
                {
                    result.TextColor = AppColors.TextOverPrimary;
                }
                

                result.UILeftVisible = !string.IsNullOrEmpty(result.LeftCommandName);
                result.UIRightVisible = !string.IsNullOrEmpty(result.RightCommandName);

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;
                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        private async void Left_Tapped(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync(nameof(Left_Tapped), async delegate ()
            {
                View view = (sender as View);
                HeaderTitleBarContext context = view?.BindingContext as HeaderTitleBarContext;
                if (context != null)
                {
                    if (context.CommandScope?.CommandProcessor != null)
                    {
                        await context.CommandScope.CommandProcessor.ExecuteCommandAsync(context.CommandScope, context.LeftCommandName, context.LeftCommandParameter, context?.DataViewItem?.DataViewModel);
                    }
                }
            });
        }
        private async void Right_Tapped(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync(nameof(Right_Tapped), async delegate ()
            {
                View view = (sender as View);
                HeaderTitleBarContext context = view?.BindingContext as HeaderTitleBarContext;
                if (context != null)
                {
                    if (context.CommandScope?.CommandProcessor != null)
                    {
                        await context.CommandScope.CommandProcessor.ExecuteCommandAsync(context.CommandScope, context.RightCommandName, context.RightCommandParameter, context?.DataViewItem?.DataViewModel);
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

        private string _leftIcon;
        public string LeftIcon
        {
            get { return _leftIcon; }
            set { SetProperty(ref _leftIcon, value); }
        }

        private string _rightIcon;
        public string RightIcon
        {
            get { return _rightIcon; }
            set { SetProperty(ref _rightIcon, value); }
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

        public string LeftCommandName { get; set; }
        public string LeftCommandParameter { get; set; }

        public string RightCommandName { get; set; }
        public string RightCommandParameter { get; set; }


        private bool _uiLeftVisible;
        public bool UILeftVisible
        {
            get { return _uiLeftVisible; }
            set { SetProperty(ref _uiLeftVisible, value); }
        }

        private bool _uiRightVisible;
        public bool UIRightVisible
        {
            get { return _uiRightVisible; }
            set { SetProperty(ref _uiRightVisible, value); }
        }
    }
}