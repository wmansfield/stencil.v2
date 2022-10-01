using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Resourcing;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class GlyphHeader : ResourceDictionary, IDataViewComponent
    {
        public GlyphHeader()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "glyphHeader";
        public const string INTERACTION_KEY_TEXT = "text";
        public const string INTERACTION_KEY_VISIBLE = "visible";

        private const string TEMPLATE_KEY = "glyphHeader";

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
                GlyphHeaderContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<GlyphHeaderContext>(configuration_json);
                }

                if(result == null)
                {
                    result = new GlyphHeaderContext();
                }

                if (result.HeightRequest <= 0)
                {
                    result.HeightRequest = -1;
                }
                if (result.DefaultFontSize <= 0)
                {
                    result.DefaultFontSize = 16;
                }

                result.UIMaxGlyphHeight = result.DefaultFontSize;

                if (result.Glyphs != null)
                {
                    foreach (GlyphInfo item in result.Glyphs)
                    {
                        if (item.FontSize <= 0)
                        {
                            item.FontSize = result.DefaultFontSize;
                        }
                        if(item.FontSize > result.UIMaxGlyphHeight)
                        {
                            result.UIMaxGlyphHeight = item.FontSize;
                        }
                        if (string.IsNullOrWhiteSpace(item.Color))
                        {
                            item.Color = result.DefaultColor;
                        }
                        if (string.IsNullOrWhiteSpace(item.FontFamily))
                        {
                            item.FontFamily = result.DefaultFontFamily;
                        }
                    }
                }

                result.UIYConstraint = Constraint.RelativeToParent(parent => parent.Height * .5);


                if (result.Margin == null)
                {
                    result.Margin = new Thickness(1); // fixes smooth scrolling
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.PrepareInteractions();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

    }

    public class GlyphHeaderContext : PreparedBindingContext, IStateResponder
    {
        public GlyphHeaderContext()
            : base("GlyphHeaderContext")
        {
            this.HeightRequest = -1;
        }

        private GlyphInfo[] _glyphs;
        public GlyphInfo[] Glyphs
        {
            get { return _glyphs; }
            set { SetProperty(ref _glyphs, value); }
        }

        private string _defaultColor;
        public string DefaultColor
        {
            get { return _defaultColor; }
            set { SetProperty(ref _defaultColor, value); }
        }

        private int _defaultFontSize;
        public int DefaultFontSize
        {
            get { return _defaultFontSize; }
            set { SetProperty(ref _defaultFontSize, value); }
        }


        private string _defaultFontFamily;
        public string DefaultFontFamily
        {
            get { return _defaultFontFamily; }
            set { SetProperty(ref _defaultFontFamily, value); }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private Thickness _padding;
        public Thickness Padding
        {
            get { return _padding; }
            set { SetProperty(ref _padding, value); }
        }

        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        public int HeightRequest { get; set; }
        public int UIMaxGlyphHeight { get; set; }
        public Constraint UIYConstraint { get; set; }

    }
    public class GlyphInfo : PropertyClass
    {
        public GlyphInfo()
        {

        }
        public GlyphInfo(char character)
        {
            this.Text = character.ToString();
        }
        private string _fontFamily;
        public string FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set { SetProperty(ref _fontFamily, value); }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set { SetProperty(ref _text, value); }
        }


        private int _fontSize;
        public int FontSize
        {
            get { return _fontSize; }
            set { SetProperty(ref _fontSize, value); }
        }


        private Thickness _padding;
        public Thickness Padding
        {
            get { return _padding; }
            set { SetProperty(ref _padding, value); }
        }

        private string _color;
        public string Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        private bool _bottom;
        public bool Bottom
        {
            get { return _bottom; }
            set
            {
                if (SetProperty(ref _bottom, value))
                {
                    this.OnPropertyChanged(nameof(UIVerticalTextAlignment));
                }
            }
        }

        private bool _top;
        public bool Top
        {
            get { return _top; }
            set
            {
                if (SetProperty(ref _top, value))
                {
                    this.OnPropertyChanged(nameof(UIVerticalTextAlignment));
                }
            }
        }

        public TextAlignment UIVerticalTextAlignment
        {
            get
            {
                if (this.Top)
                {
                    return TextAlignment.Start;
                }
                else if (this.Bottom)
                {
                    return TextAlignment.End;
                }
                else
                {
                    return TextAlignment.Center;
                }
            }
        }
    }
}