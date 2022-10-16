using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using System.Linq;

namespace Stencil.Maui.Views.Standard.v1_0
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
                        if (string.IsNullOrWhiteSpace(item.TextColor))
                        {
                            item.TextColor = result.DefaultColor;
                        }
                        if (string.IsNullOrWhiteSpace(item.FontFamily))
                        {
                            item.FontFamily = result.DefaultFontFamily;
                        }
                    }
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.PrepareInteractions();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        private void ApplyChildPosition(GlyphHeaderContext context, AbsoluteLayout parent, StackLayout child)
        {
            CoreUtility.ExecuteMethod("ApplyChildPosition", delegate ()
            {
                context.UIAppliedHeight = parent.Height;
                context.UIAppliedWidth = parent.Width;

                Rect bounds = context.ComputeLayoutBounds(parent, child);

                parent.SetLayoutBounds(child, bounds);
                parent.SetLayoutFlags(child, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None);
            });
        }

        private void glyphStackRoot_SizeChanged(object sender, EventArgs e)
        {
            CoreUtility.ExecuteMethod("glyphStackRoot_SizeChanged", delegate ()
            {
                StackLayout child = sender as StackLayout;
                AbsoluteLayout parent = child.Parent as AbsoluteLayout;
                GlyphHeaderContext context = child.BindingContext as GlyphHeaderContext;

                if (child != null && parent != null && context != null)
                {
                    if(context.UIAppliedHeight != parent.Height || context.UIAppliedWidth != parent.Width)
                    {
                        this.ApplyChildPosition(context, parent, child);
                    }
                }
            });
            
        }
        private void glyphLayoutRoot_SizeChanged(object sender, EventArgs e)
        {
            CoreUtility.ExecuteMethod("glyphLayoutRoot_SizeChanged", delegate ()
            {
                AbsoluteLayout parent = sender as AbsoluteLayout;
                GlyphHeaderContext context = parent.BindingContext as GlyphHeaderContext;
                if(context != null)
                {
                    bool hasAppliedChild = context.UIAppliedHeight != 0;
                    if (hasAppliedChild)
                    {
                        if (context.UIAppliedHeight != parent.Height || context.UIAppliedWidth != parent.Width)
                        {
                            StackLayout child = parent.Children.FirstOrDefault() as StackLayout;
                            if(child != null)
                            {
                                this.ApplyChildPosition(context, parent, child);
                            }
                        }
                    }
                }
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
        public double UIAppliedHeight { get; set; }
        public double UIAppliedWidth { get; set; }

        public Rect ComputeLayoutBounds(View parentView, View childView)
        {
            return CoreUtility.ExecuteFunction("ComputeLayoutBounds", delegate ()
            {
                return new Rect()
                {
                    X = this.ComputeX(parentView, childView, this.Margin),
                    Y = this.ComputeY(parentView, childView, this.Margin),
                    Width = childView.Width,
                    Height = childView.Height
                };
            });
        }
        
        protected double ComputeY(View parent, View child, Thickness childMargin)
        {
            return CoreUtility.ExecuteFunction("ComputeY", delegate ()
            {
                double result = 0;
                if (child.Height > parent.Height)
                {
                    result = -((child.Height - parent.Height) / 2);
                }
                else if (child.Height < parent.Height)
                {
                    result = ((parent.Height - child.Height) / 2);
                }
                result += childMargin.Top + childMargin.Bottom;
                return result;
            });
        }
        protected double ComputeX(View parent, View child, Thickness childMargin)
        {
            return CoreUtility.ExecuteFunction("ComputeX", delegate ()
            {
                double result = 0;
                if (child.Width > parent.Width)
                {
                    result = -((child.Width - parent.Width) / 2);
                }
                else if (child.Width < parent.Width)
                {
                    result = ((parent.Width - child.Width) / 2);
                }
                result += childMargin.Left + childMargin.Right;

                return result;
            });
        }

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

        private string _textColor;
        public string TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
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