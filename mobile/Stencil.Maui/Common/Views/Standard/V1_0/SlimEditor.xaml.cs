using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.Windows.Input;
using System;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class SlimEditor : ResourceDictionary, IDataViewComponent
    {
        #region Constructor
        public SlimEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Constants

        public const string COMPONENT_NAME = "slimEditor";
        private const string TEMPLATE_KEY = "slimEditor";

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
                SlimEditorContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<SlimEditorContext>(configuration_json);
                }
                if (result == null)
                {
                    result = new SlimEditorContext();
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
                    result.FontFamily = "SansRegular";
                }
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                commandScope.RegisterCommandField(result);

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        #endregion
    }

    public class SlimEditorContext : PreparedBindingContext, ICommandField
    {
        #region Constructor

        public SlimEditorContext()
            : base(nameof(SlimEditorContext))
        {
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

        private string _keyboardType;
        public string KeyboardType
        {
            get { return _keyboardType; }
            set { SetProperty(ref _keyboardType, value); }
        }

        private bool _spellCheckEnabled = true;
        public bool SpellCheckEnabled
        {
            get { return _spellCheckEnabled; }
            set { SetProperty(ref _spellCheckEnabled, value); }
        }

        private bool _textPredictionEnabled = true;
        public bool TextPredictionEnabled
        {
            get { return _textPredictionEnabled; }
            set { SetProperty(ref _textPredictionEnabled, value); }
        }

        private bool _suppressBottomLine = true;
        public bool SuppressBottomLine
        {
            get { return _suppressBottomLine; }
            set { SetProperty(ref _suppressBottomLine, value); }
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

        public bool UIVisible
        {
            get
            {
                return !this.Hidden;
            }
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
