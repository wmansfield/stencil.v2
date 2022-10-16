using Newtonsoft.Json;
using Stencil.Common;
using Stencil.Maui.Commanding;
using Stencil.Maui.Platform;
using Stencil.Maui.Resourcing;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class DropDown : ResourceDictionary, IDataViewComponent
    {
        #region Constructor

        public DropDown()
        {
            InitializeComponent();
        }

        #endregion

        #region Constants

        public const string COMPONENT_NAME = "dropDown";
        private const string TEMPLATE_KEY = "dropDown";

        #endregion

        #region DataViewComponent

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
                DropDownContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<DropDownContext>(configuration_json);
                }
                if (result == null)
                {
                    result = new DropDownContext();
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                commandScope.RegisterCommandField(result);

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        private async void DropDown_Clicked(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync($"{COMPONENT_NAME}.DropDown_Clicked", async delegate ()
            {
                View view = (sender as View);
                DropDownContext context = view?.BindingContext as DropDownContext;
                if (context != null)
                {
                    DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();

                    string display = await NativeApplication.Alerts.ActionSheetAsync(context.Label, context.DropDownCancelText, null, null, context.AvailableValues.Select(x => x.display).ToArray());
                    if (display != context.DropDownCancelText)
                    {
                        DisplayPair match = context.AvailableValues.FirstOrDefault(x => x.display == display);
                        if (match != context.SelectedItem)
                        {
                            context.SelectedItem = match;
                        }
                    }
                }
            });

        }

        #endregion
    }

    
    public class DropDownContext : PreparedBindingContext, ICommandField
    {
        #region Constructor

        public DropDownContext()
            : base(nameof(DropDownContext))
        {
        }

        #endregion

        #region ICommandField Properties

        public string GroupName { get; set; }
        public string FieldName { get; set; }
        public bool IsRequired { get; set; }

        #endregion

        #region Data Properties

        private DisplayPair _selectedItem;
        public DisplayPair SelectedItem
        {
            get { return _selectedItem; }
            set 
            { 
                if(SetProperty(ref _selectedItem, value))
                {
                    if(value != null)
                    {
                        this.SelectedDisplay = value.display;
                    }
                    else
                    {
                        this.SelectedDisplay = string.Empty;
                    }
                }
            }
        }

        private List<DisplayPair> _availableValues;
        public List<DisplayPair> AvailableValues
        {
            get { return _availableValues; }
            set { SetProperty(ref _availableValues, value); }
        }

        private string _selectedDisplay;
        public string SelectedDisplay
        {
            get { return _selectedDisplay; }
            set { SetProperty(ref _selectedDisplay, value); }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private string _textColor = AppColors.TextOverBackground;
        public string TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

        private string _buttonBackgroundColor;
        public string ButtonBackgroundColor
        {
            get { return _buttonBackgroundColor; }
            set { SetProperty(ref _buttonBackgroundColor, value); }
        }

        private string _dropDownIcon = FontAwesome.fa_caret_down;
        public string DropDownIcon
        {
            get { return _dropDownIcon; }
            set { SetProperty(ref _dropDownIcon, value); }
        }


        private string _dropDownCancelText = "Cancel";//TODO:MUST: Localize
        public string DropDownCancelText
        {
            get { return _dropDownCancelText; }
            set { SetProperty(ref _dropDownCancelText, value); }
        }

        private string _placeholder;
        public string Placeholder
        {
            get { return _placeholder; }
            set { SetProperty(ref _placeholder, value); }
        }

        private Thickness _padding = new Thickness();
        public Thickness Padding
        {
            get { return _padding; }
            set { SetProperty(ref _padding, value); }
        }

        private string _placeholderColor = AppColors.TextOverBackgroundMuted;
        public string PlaceholderColor
        {
            get { return _placeholderColor; }
            set { SetProperty(ref _placeholderColor, value); }
        }
        private string _inputBackgroundColor;
        public string InputBackgroundColor
        {
            get { return _inputBackgroundColor; }
            set { SetProperty(ref _inputBackgroundColor, value); }
        }
        private bool _borderless;
        public bool Borderless
        {
            get { return _borderless; }
            set { SetProperty(ref _borderless, value); }
        }

        #endregion

        #region UI Properties

        private bool _uiEntryFocused;
        public bool UIEntryFocused
        {
            get { return _uiEntryFocused; }
            set { SetProperty(ref _uiEntryFocused, value); }
        }

        #endregion


        #region ICommandField Methods

        public string GetFieldValue()
        {
            return this.SelectedItem?.id;
        }
        public void SetFieldValue(string value)
        {
            if(value == null)
            {
                value = string.Empty;
            }
            DisplayPair selected = this.AvailableValues.FirstOrDefault(x => value.Equals(x.id, StringComparison.OrdinalIgnoreCase));
            this.SelectedItem = selected;
        }
        public Task<string> ValidateUserInputAsync()
        {
            return base.ExecuteFunction(nameof(ValidateUserInputAsync), delegate ()
            {
                if (this.IsRequired)
                {
                    string value = this.GetFieldValue();
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return Task.FromResult("Value required");//TODO:MUST: Localize
                    }
                }
                return Task.FromResult((string)null);
            });
        }

        #endregion

    }
}
