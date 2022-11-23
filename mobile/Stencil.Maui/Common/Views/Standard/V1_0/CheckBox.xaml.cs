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
    public partial class CheckBox : ResourceDictionary, IDataViewComponent
    {
        #region Constructor

        public CheckBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Constants

        public const string COMPONENT_NAME = "checkBox";
        private const string TEMPLATE_KEY = "checkBox";

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
                CheckBoxContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<CheckBoxContext>(configuration_json);
                }
                if (result == null)
                {
                    result = new CheckBoxContext();
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
                if(result.IconFontSize <= 0)
                {
                    result.IconFontSize = 18;
                }
                if (string.IsNullOrWhiteSpace(result.FontFamily))
                {
                    result.FontFamily = "SansMedium";
                }
                if (string.IsNullOrWhiteSpace(result.IconBlank))
                {
                    result.IconBlank = FontAwesome.fa_square_o;
                }
                if (string.IsNullOrWhiteSpace(result.IconSelected))
                {
                    result.IconSelected = FontAwesome.fa_check_square_o;
                }
                if (string.IsNullOrWhiteSpace(result.IconUnselected))
                {
                    result.IconUnselected = FontAwesome.fa_square_o;
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                commandScope.RegisterCommandField(result);

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        private void CheckBox_Clicked(object sender, EventArgs e)
        {
            CoreUtility.ExecuteMethod("CheckBox.CheckBox_Clicked", delegate ()
            {
                View view = (sender as View);
                CheckBoxContext context = view?.BindingContext as CheckBoxContext;
                if (context != null)
                {
                    NativeApplication.Keyboard?.TryHideKeyboard();

                    context.Selected = !context.Selected.GetValueOrDefault();
                }
            });

        }

        #endregion
    }

    
    public class CheckBoxContext : PreparedBindingContext, ICommandField
    {
        #region Constructor

        public CheckBoxContext()
            : base("CheckBoxContext")
        {
        }

        #endregion

        #region ICommandField Properties

        public string GroupName { get; set; }
        public string FieldName { get; set; }
        public bool IsRequired { get; set; }

        #endregion

        #region Data Properties

        private bool? _selected;
        public bool? Selected
        {
            get { return _selected; }
            set 
            { 
                if(SetProperty(ref _selected, value))
                {
                    this.RaisePropertyChanged(nameof(UIIcon));
                }
            }
        }


        public string UIIcon
        {
            get 
            {
                if (!this.Selected.HasValue)
                {
                    return this.IconBlank;
                }
                else if (this.Selected == true)
                {
                    return this.IconSelected;
                }
                else
                {
                    return this.IconUnselected;
                }
            }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        private string _backgroundColor = AppColors.Transparent;
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

        private string _iconBlank;
        public string IconBlank
        {
            get { return _iconBlank; }
            set { SetProperty(ref _iconBlank, value); }
        }

        private string _iconSelected;
        public string IconSelected
        {
            get { return _iconSelected; }
            set { SetProperty(ref _iconSelected, value); }
        }

        private string _iconUnselected;
        public string IconUnselected
        {
            get { return _iconUnselected; }
            set { SetProperty(ref _iconUnselected, value); }
        }

        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        private string _fontFamily = "SansRegular";
        public string FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set { SetProperty(ref _fontFamily, value); }
        }

        

        private int _iconFontSize;
        public int IconFontSize
        {
            get { return _iconFontSize; }
            set { SetProperty(ref _iconFontSize, value); }
        }

        private double _fontSize;
        public double FontSize
        {
            get { return _fontSize; }
            set { SetProperty(ref _fontSize, value); }
        }
        #endregion


        #region ICommandField Methods

        public string GetFieldValue()
        {
            return this.Selected.GetValueOrDefault().ToString();
        }
        public void SetFieldValue(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                this.Selected = null;
            }
            else
            {
                this.Selected = value.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }
        public Task<string> ValidateUserInputAsync()
        {
            return base.ExecuteFunction(nameof(ValidateUserInputAsync), delegate ()
            {
                if (this.IsRequired)
                {
                    string value = this.GetFieldValue();
                    if (!"true".Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        return Task.FromResult(this.API.Localize(StencilLanguageTokens.Stencil_Validation_SelectionRequired.ToString(), "Selection Required For: {0}", this.FieldName, this.Label));
                    }
                }
                return Task.FromResult((string)null);
            });
        }

        #endregion

    }
}
