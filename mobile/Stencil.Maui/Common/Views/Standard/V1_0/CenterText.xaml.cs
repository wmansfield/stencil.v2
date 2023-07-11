using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Stencil.Common.Views;
using Stencil.Maui.Data;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class CenterText : ResourceDictionary, IDataViewComponent
    {
        public CenterText()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "centerText";

        private const string TEMPLATE_KEY = "centerText";

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
                CenterTextContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<CenterTextContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new CenterTextContext();
                }

                if(string.IsNullOrWhiteSpace(result.ContentWidth))
                {
                    result.ContentWidth = "*";
                }
                
                if(result.FontSize <= 0)
                {
                    result.FontSize = 16;
                }
                if (string.IsNullOrWhiteSpace(result.FontFamily))
                {
                    result.FontFamily = "SansRegular";
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.EnsureInteractionsPrepared();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class CenterTextContext : PreparedBindingContext, IStateResponder
    {
        public CenterTextContext()
            : base(nameof(CenterTextContext))
        {

        }

        public const string INTERACTION_KEY_TEXT = "text";
        public const string INTERACTION_KEY_HIDDEN = "hidden";

        private string _fontFamily = "SansRegular";
        public string FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set { SetProperty(ref _fontFamily, value); }
        }

        private int _fontSize;
        public int FontSize
        {
            get { return _fontSize; }
            set { SetProperty(ref _fontSize, value); }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _textColor = AppColors.PrimaryBlack;
        public string TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

        private string _backgroundColor = AppColors.Transparent;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }


        private string _contentWidth;
        public string ContentWidth
        {
            get { return _contentWidth; }
            set { SetProperty(ref _contentWidth, value); }
        }

        private ThicknessInfo _padding = new ThicknessInfo();
        public ThicknessInfo Padding
        {
            get { return _padding; }
            set
            {
                if (SetProperty(ref _padding, value))
                {
                    this.UIPadding = value.ToThickness();
                }
            }
        }
        private ThicknessInfo _margin;
        public ThicknessInfo Margin
        {
            get { return _margin; }
            set
            {
                if (SetProperty(ref _margin, value))
                {
                    this.UIMargin = value.ToThickness();
                }
            }
        }

        private bool _center;
        public bool Center
        {
            get { return _center; }
            set
            {
                if (SetProperty(ref _center, value))
                {
                    this.OnPropertyChanged(nameof(UITextAlignment));
                }
            }
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

        private Thickness _uiPadding = new Thickness();
        public Thickness UIPadding
        {
            get { return _uiPadding; }
            protected set { SetProperty(ref _uiPadding, value); }
        }

        private Thickness _uiMargin = new Thickness();
        public Thickness UIMargin
        {
            get { return _uiMargin; }
            protected set { SetProperty(ref _uiMargin, value); }
        }

        public TextAlignment UITextAlignment
        {
            get
            {
                if(this.Center)
                {
                    return TextAlignment.Center;
                }
                else
                {
                    return TextAlignment.Start;
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


        protected override void ApplyStateValue(string group, string state_key, string state, string value_key, string value)
        {
            base.ExecuteMethod(nameof(ApplyStateValue), delegate ()
            {
                switch (value_key)
                {
                    case INTERACTION_KEY_TEXT:
                        this.Text = value;
                        break;
                    case INTERACTION_KEY_HIDDEN:
                        if (!string.IsNullOrEmpty(value))
                        {
                            this.Hidden = value.Equals("true", StringComparison.OrdinalIgnoreCase);
                        }
                        break;
                    default:
                        break;
                }
            });
        }
    }
}