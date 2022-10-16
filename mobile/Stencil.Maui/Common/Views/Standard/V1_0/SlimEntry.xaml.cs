using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class SlimEntry : ResourceDictionary, IDataViewComponent
    {
        #region Constructor
        public SlimEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Constants

        public const string COMPONENT_NAME = "slimEntry";
        private const string TEMPLATE_KEY = "slimEntry";

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
                SlimEntryContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<SlimEntryContext>(configuration_json);
                }
                if (result == null)
                {
                    result = new SlimEntryContext();
                }
                if (result.FontSize <= 0)
                {
                    try
                    {
                        result.FontSize = (int)Application.Current.Resources["FontSizeMedium"];
                    }
                    catch
                    {
                        result.FontSize = 14;
                    }
                }
                if (string.IsNullOrWhiteSpace(result.FontFamily))
                {
                    result.FontFamily = "SansMedium";
                }
                result.UIAsPassword = result.IsPassword; // reset password flag

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.PrepareInteractions();

                commandScope.RegisterCommandField(result);

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        #endregion
    }

    public class SlimEntryContext : PreparedBindingContext, ICommandField, IStateResponder
    {
        #region Constructor

        public SlimEntryContext()
            : base(nameof(SlimEntryContext))
        {
            this.ApplyPasswordVisibility();
        }

        #endregion

        #region State Responders

        public const string INTERACTION_KEY_VALUE = "value";
        public const string INTERACTION_KEY_HIDDEN = "hidden";

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
                    case INTERACTION_KEY_VALUE:
                        this.SetFieldValue(value);
                        break;
                    default:
                        break;
                }
            });
        }

        #endregion

        #region ICommandField Properties

        public string GroupName { get; set; }
        public string FieldName { get; set; }
        public bool IsRequired { get; set; }

        #endregion

        #region Data Properties

        private string _input;
        public string Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
        }

        private string _placeholder;
        public string Placeholder
        {
            get { return _placeholder; }
            set { SetProperty(ref _placeholder, value); }
        }

        private bool _isPassword;
        public bool IsPassword
        {
            get { return _isPassword; }
            set { SetProperty(ref _isPassword, value); }
        }


        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { SetProperty(ref _isReadOnly, value); }
        }
        

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private string _inputBackgroundColor;
        public string InputBackgroundColor
        {
            get { return _inputBackgroundColor; }
            set { SetProperty(ref _inputBackgroundColor, value); }
        }
        private string _textColor = AppColors.TextOverBackground;
        public string TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

        private string _placeholderColor = AppColors.TextOverBackgroundMuted;
        public string PlaceholderColor
        {
            get { return _placeholderColor; }
            set { SetProperty(ref _placeholderColor, value); }
        }

        private Thickness _padding = new Thickness();
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

        private string _fontFamily;
        public string FontFamily
        {
            get { return _fontFamily; }
            set { SetProperty(ref _fontFamily, value); }
        }

        private double _fontSize;
        public double FontSize
        {
            get { return _fontSize; }
            set { SetProperty(ref _fontSize, value); }
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

        #endregion

        #region Binding Properties

        private bool _uiEntryFocused;
        public bool UIEntryFocused
        {
            get { return _uiEntryFocused; }
            set { SetProperty(ref _uiEntryFocused, value); }
        }


        private bool _uiAsPassword;
        public bool UIAsPassword
        {
            get { return _uiAsPassword; }
            set { SetProperty(ref _uiAsPassword, value); }
        }

        private bool _uiPasswordVisible;
        public bool UIPasswordVisible
        {
            get { return _uiPasswordVisible; }
            set { SetProperty(ref _uiPasswordVisible, value); }
        }

        private string _uiPasswordIcon;
        public string UIPasswordIcon
        {
            get { return _uiPasswordIcon; }
            set { SetProperty(ref _uiPasswordIcon, value); }
        }

        public bool UIVisible
        {
            get
            {
                return !this.Hidden;
            }
        }


        public Command _UITogglePasswordVisibilityCommand;
        public Command UITogglePasswordVisibilityCommand
        {
            get
            {
                return _UITogglePasswordVisibilityCommand ?? (_UITogglePasswordVisibilityCommand = new Command(UITogglePasswordVisibility));
            }
        }
        protected void UITogglePasswordVisibility()
        {
            base.ExecuteMethod(nameof(UITogglePasswordVisibility), delegate ()
            {
                this.UIPasswordVisible = !this.UIPasswordVisible;
                this.ApplyPasswordVisibility();
            });
        }
        protected void ApplyPasswordVisibility()
        {
            base.ExecuteMethod(nameof(ApplyPasswordVisibility), delegate ()
            {
                if (this.UIPasswordVisible)
                {
                    this.UIPasswordIcon = FontAwesome.fa_eye_open;
                    this.UIAsPassword = false;
                }
                else
                {
                    this.UIPasswordIcon = FontAwesome.fa_eye_slashed;
                    this.UIAsPassword = true;
                }
            });
        }

        #endregion

        #region ICommandField Methods

        public string GetFieldValue()
        {
            return this.Input;
        }
        public void SetFieldValue(string value)
        {
            this.Input = value;
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
