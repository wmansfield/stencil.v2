using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Widget;
using Stencil.Common.Markdown;
using Stencil.Forms.Droid.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Forms.Droid.Markdown
{
    [Preserve]
    public class MarkdownLinearLayout : BaseLinearLayout
    {
        #region Constructor

        public MarkdownLinearLayout(Context context)
            : base(context)
        {
            this.TrackPrefix = "MarkdownLinearLayout";

        }

        public MarkdownLinearLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            this.TrackPrefix = "MarkdownLinearLayout";
        }

        public MarkdownLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            this.TrackPrefix = "MarkdownLinearLayout";
        }

        public MarkdownLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
            this.TrackPrefix = "MarkdownLinearLayout";
        }
        public MarkdownLinearLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            this.TrackPrefix = "MarkdownLinearLayout";
        }

        #endregion

        #region Constants

        private const int HEADER_EXTRA_FONT_SIZE = 2;
        private const string BULLET_NUMBER_FORMAT = "{0}. ";
        private const string BULLET_PREFIX = "- ";
        private const string BLOCK_QUOTE_PREFIX = "“ ";
        private const string DIVIDER_TEXT = "----";
        private const int DIVIDER_FONT_SIZE = 10;
        private const int BULLET_NUMBER_BODY_INDENT = 15;
        private const int BULLET_DASH_BODY_INDENT = 10;
        private const int BLOCKQUOTE_BODY_INDENT = 10;

        #endregion

        #region Properties

        public Action ClickAnythingAction { get; set; }
        public Action LongClickAnythingAction { get; set; }

        private Typeface _fontRegular;
        protected Typeface FontRegular
        {
            get
            {
                if (_fontRegular == null)
                {
                    _fontRegular = FontLoader.GetFont(Context.Assets, StencilPreferences.FONT_REGULAR);
                }
                return _fontRegular;
            }
        }

        private Typeface _fontItalic;
        protected Typeface FontItalic
        {
            get
            {
                if (_fontItalic == null)
                {
                    _fontItalic = FontLoader.GetFont(Context.Assets, StencilPreferences.FONT_ITALIC);
                }
                return _fontItalic;
            }
        }

        private Typeface _fontBold;
        protected Typeface FontBold
        {
            get
            {
                if (_fontBold == null)
                {
                    _fontBold = FontLoader.GetFont(Context.Assets, StencilPreferences.FONT_BOLD);
                }
                return _fontBold;
            }
        }

        private Typeface _fontBoldItalic;
        protected Typeface FontBoldItalic
        {
            get
            {
                if (_fontBoldItalic == null)
                {
                    _fontBoldItalic = FontLoader.GetFont(Context.Assets, StencilPreferences.FONT_BOLD_ITALIC);
                }
                return _fontBoldItalic;
            }
        }

        #endregion

        #region Public Methods

        public void ClearText()
        {
            base.ExecuteMethod(nameof(ClearText), delegate ()
            {
                this.ClickAnythingAction = null;
                this.LongClickAnythingAction = null;

                this.RemoveAllViews();
            });
        }
        public void SetText(IMarkdownHost host, List<MarkdownSection> sections)
        {
            base.ExecuteMethod(nameof(SetText), delegate ()
            {
                this.ClearText();

                this.ClickAnythingAction = host.AnythingTapped;

                List<View> viewsToAdd = new List<View>();

                if (sections == null || sections.Count == 0)
                {
                    // do nothing?
                }
                else
                {
                    foreach (MarkdownSection section in sections)
                    {
                        MarkdownData data = null;
                        switch (section.kind)
                        {
                            case MarkdownSectionKind.header:
                                data = this.GenerateMarkdownText(string.Empty, section.text, section.annotations, host.FontSize + HEADER_EXTRA_FONT_SIZE, 0, true, false, false);
                                data.Text.WireUpLinks(string.Empty, data.Links, StencilPreferences.COLOR_Markdown_Link_Foreground.ConvertHexToColor(), (span, view) => { host.LinkTapped(span.Argument); });

                                TextView headerView = this.GenerateTextView(host.FontSize + HEADER_EXTRA_FONT_SIZE);
                                headerView.TextFormatted = data.Text;
                                viewsToAdd.Add(headerView);
                                break;
                            case MarkdownSectionKind.text:
                                data = this.GenerateMarkdownText(string.Empty, section.text, section.annotations, host.FontSize, 0, false, false, false);
                                data.Text.WireUpLinks(string.Empty, data.Links, StencilPreferences.COLOR_Markdown_Link_Foreground.ConvertHexToColor(), (span, view) => { host.LinkTapped(span.Argument); });

                                TextView textView = this.GenerateTextView(host.FontSize);
                                textView.TextFormatted = data.Text;
                                viewsToAdd.Add(textView);
                                break;
                            case MarkdownSectionKind.bullet_number:
                                int number = 0;
                                foreach (AnnotatedTextItem item in section.items)
                                {
                                    number++;
                                    data = this.GenerateMarkdownText(string.Format(BULLET_NUMBER_FORMAT, number), item.text, item.annotations, host.FontSize, BULLET_NUMBER_BODY_INDENT, false, false, false);
                                    data.Text.WireUpLinks(string.Format(BULLET_NUMBER_FORMAT, number), data.Links, StencilPreferences.COLOR_Markdown_Link_Foreground.ConvertHexToColor(), (span, view) => { host.LinkTapped(span.Argument); });

                                    TextView bulletNumberView = this.GenerateTextView(host.FontSize);
                                    bulletNumberView.TextFormatted = data.Text;
                                    viewsToAdd.Add(bulletNumberView);
                                }
                                break;
                            case MarkdownSectionKind.bullet_text:
                                foreach (AnnotatedTextItem item in section.items)
                                {
                                    data = this.GenerateMarkdownText(BULLET_PREFIX, item.text, item.annotations, host.FontSize, BULLET_DASH_BODY_INDENT, false, false, false);
                                    data.Text.WireUpLinks(BULLET_PREFIX, data.Links, StencilPreferences.COLOR_Markdown_Link_Foreground.ConvertHexToColor(), (span, view) => { host.LinkTapped(span.Argument); });

                                    TextView bulletTextView = this.GenerateTextView(host.FontSize);
                                    bulletTextView.TextFormatted = data.Text;
                                    viewsToAdd.Add(bulletTextView);
                                }
                                break;
                            case MarkdownSectionKind.code_block:
                                data = this.GenerateMarkdownText(string.Empty, section.text, section.annotations, host.FontSize, 0, false, true, false);
                                data.Text.WireUpLinks(string.Empty, data.Links, StencilPreferences.COLOR_Markdown_Link_Foreground.ConvertHexToColor(), (span, view) => { host.LinkTapped(span.Argument); });

                                TextView codeView = this.GenerateTextView(host.FontSize);
                                codeView.SetBackgroundColor(StencilPreferences.COLOR_Markdown_Code_Background.ConvertHexToColor());
                                codeView.TextFormatted = data.Text;
                                viewsToAdd.Add(codeView);
                                break;
                            case MarkdownSectionKind.block_quote:
                                data = this.GenerateMarkdownText(BLOCK_QUOTE_PREFIX, section.text, section.annotations, host.FontSize, BLOCKQUOTE_BODY_INDENT, false, false, false);
                                data.Text.WireUpLinks(BLOCK_QUOTE_PREFIX, data.Links, StencilPreferences.COLOR_Markdown_Link_Foreground.ConvertHexToColor(), (span, view) => { host.LinkTapped(span.Argument); });

                                TextView blockView = this.GenerateTextView(host.FontSize);
                                blockView.TextFormatted = data.Text;
                                viewsToAdd.Add(blockView);
                                break;
                            case MarkdownSectionKind.image:
                            case MarkdownSectionKind.video:
                                MarkdownMediaView mediaView = new MarkdownMediaView(this.Context);
                                mediaView.SetMedia(host, section, 0);//TODO:MUST Handle totalHorizontalMargin properly
                                viewsToAdd.Add(mediaView);
                                break;
                            case MarkdownSectionKind.divider:
                                if (!host.SuppressDivider)
                                {
                                    section.text = DIVIDER_TEXT;

                                    data = this.GenerateMarkdownText(string.Empty, section.text, section.annotations, DIVIDER_FONT_SIZE, 0, false, false, true);
                                    data.Text.WireUpLinks(string.Empty, data.Links, StencilPreferences.COLOR_Markdown_Link_Foreground.ConvertHexToColor(), (span, view) => { host.LinkTapped(span.Argument); });

                                    TextView dividerLabel = this.GenerateTextView(host.FontSize);
                                    dividerLabel.TextFormatted = data.Text;
                                    viewsToAdd.Add(dividerLabel);
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }

                foreach (View item in viewsToAdd)
                {
                    this.AddView(item);
                }

            });
        }

        #endregion

        #region Protected Methods

        protected TextView GenerateTextView(float fontSize)
        {
            return base.ExecuteFunction("GenerateTextView", delegate ()
            {
                TextView textView = new TextView(this.Context);
                textView.AutoLinkMask = Android.Text.Util.MatchOptions.All;
                textView.SetCustomFont(StencilPreferences.FONT_REGULAR);
                textView.SetTextColor(StencilPreferences.COLOR_Markdown_Text_Foreground.ConvertHexToColor());
                textView.SetTextSize(ComplexUnitType.Sp, fontSize);
                textView.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                textView.MovementMethod = LinkMovementMethod.Instance;
                if (this.ClickAnythingAction != null)
                {
                    textView.Click += AnyTextView_Click;
                }
                if (this.LongClickAnythingAction != null)
                {
                    textView.LongClick += LongAnyTextView_Click;
                }
                return textView;
            });

        }

        private void AnyTextView_Click(object sender, EventArgs e)
        {
            base.ExecuteMethod("AnyTextView_Click", delegate ()
            {
                this.ClickAnythingAction?.Invoke();
            });
        }
        private void LongAnyTextView_Click(object sender, EventArgs e)
        {
            base.ExecuteMethod("LongAnyTextView_Click", delegate ()
            {
                this.LongClickAnythingAction?.Invoke();
            });
        }
        protected MarkdownData GenerateMarkdownText(string prefix, string markdownText, List<TextAnnotation> annotations, float fontSize, int bodyIndent, bool forceBold, bool forceCode, bool forceCenter)
        {
            return base.ExecuteFunction("GenerateMarkdownText", delegate ()
            {
                if (prefix == null)
                {
                    prefix = string.Empty;
                }
                float fontSizeSp = fontSize.ToSp(this.Context);

                if (annotations == null || annotations.Count == 0)
                {
                    string allText = (prefix + markdownText).TrimSafe();
                    SpannableString spannable = new SpannableString(allText);
                    spannable.SetRangeFont(0, allText.Length, this.FontRegular, fontSizeSp);
                    spannable.SetRangeColor(0, allText.Length, null, StencilPreferences.COLOR_Markdown_Text_Foreground.ConvertHexToColor());

                    this.ApplyColoring(spannable, 0, allText.Length, forceCode, false);
                    this.ApplyIndents(spannable, 0, allText.Length, bodyIndent, forceCode, false);
                    this.ApplyFont(spannable, 0, allText.Length, fontSizeSp, forceBold, false, false);
                    this.ApplyAlignment(spannable, 0, allText.Length, forceCenter);

                    spannable.EnsureEmojis(this.Context, (int)fontSizeSp);
                    return new MarkdownData(spannable, new List<TextAnnotation>());
                }
                else
                {
                    Dictionary<int, List<TextAnnotation>> startAnnotations = new Dictionary<int, List<TextAnnotation>>();
                    Dictionary<int, List<TextAnnotation>> endAnnotations = new Dictionary<int, List<TextAnnotation>>();

                    foreach (TextAnnotation item in annotations)
                    {
                        if (!startAnnotations.ContainsKey(item.start))
                        {
                            startAnnotations[item.start] = new List<TextAnnotation>();
                        }
                        startAnnotations[item.start].Add(item);

                        if (!endAnnotations.ContainsKey(item.end))
                        {
                            endAnnotations[item.end] = new List<TextAnnotation>();
                        }
                        endAnnotations[item.end].Add(item);

                    }

                    int currentBold = forceBold ? 100 : 0;
                    int currentItalic = 0;
                    int currentUnderline = 0;
                    int currentCode = forceCode ? 100 : 0;
                    int currentHighlight = 0;

                    List<TextAnnotation> linkAnnotations = new List<TextAnnotation>();

                    SpannableString spannable = new SpannableString(prefix + markdownText);
                    spannable.SetRangeFont(0, markdownText.Length, this.FontRegular, fontSizeSp);
                    spannable.SetRangeColor(0, markdownText.Length, null, StencilPreferences.COLOR_Markdown_Text_Foreground.ConvertHexToColor());

                    List<char> currentText = new List<char>(markdownText.Length);

                    int ixLastCommit = 0;
                    int ixStart = 0;
                    int ixEnd = 0;
                    for (int i = 0; i < markdownText.Length; i++)
                    {
                        ixStart = ixLastCommit;
                        if (ixLastCommit > 0)
                        {
                            ixStart += prefix.Length;
                        }
                        ixEnd = ixStart + currentText.Count;

                        if (endAnnotations.ContainsKey(i))
                        {
                            if (currentText.Count > 0)
                            {
                                // commit current
                                this.ApplyColoring(spannable, ixStart, ixEnd, (currentCode > 0), (currentHighlight > 0));
                                this.ApplyIndents(spannable, ixStart, ixEnd, bodyIndent, forceCode, false);
                                this.ApplyFont(spannable, ixStart, ixEnd, fontSizeSp, (currentBold > 0), (currentItalic > 0), (currentUnderline > 0));
                                this.ApplyAlignment(spannable, ixStart, ixEnd, forceCenter);

                                currentText.Clear();
                                ixLastCommit = i;
                            }

                            // set up the next
                            List<TextAnnotation> matchingAnnotations = endAnnotations[i].ToList(); // outer ones first
                            foreach (TextAnnotation item in matchingAnnotations)
                            {
                                switch (item.type)
                                {
                                    case "link":
                                        // nothing here
                                        break;
                                    case "bold":
                                        currentBold = Math.Max(0, currentBold - 1);
                                        break;
                                    case "italic":
                                        currentItalic = Math.Max(0, currentItalic - 1);
                                        break;
                                    case "underline":
                                        currentUnderline = Math.Max(0, currentUnderline - 1);
                                        break;
                                    case "code":
                                        currentCode = Math.Max(0, currentCode - 1);
                                        break;
                                    case "highlight":
                                        currentHighlight = Math.Max(0, currentHighlight - 1);
                                        break;
                                    default:
                                        // no special close
                                        break;
                                }
                            }
                        }

                        if (startAnnotations.ContainsKey(i))
                        {
                            if (currentText.Count > 0)
                            {
                                // apply current
                                this.ApplyColoring(spannable, ixStart, ixEnd, (currentCode > 0), (currentHighlight > 0));
                                this.ApplyIndents(spannable, ixStart, ixEnd, bodyIndent, forceCode, false);
                                this.ApplyFont(spannable, ixStart, ixEnd, fontSizeSp, (currentBold > 0), (currentItalic > 0), (currentUnderline > 0));
                                this.ApplyAlignment(spannable, ixStart, ixEnd, forceCenter);

                                currentText.Clear();
                                ixLastCommit = i;
                            }

                            // start new styles
                            List<TextAnnotation> matchingAnnotations = startAnnotations[i].ToList();

                            // extract font details first
                            foreach (TextAnnotation item in matchingAnnotations)
                            {
                                switch (item.type)
                                {
                                    case "bold":
                                        currentBold++;
                                        break;
                                    case "italic":
                                        currentItalic++;
                                        break;
                                    case "underline":
                                        currentUnderline++;
                                        break;
                                    case "code":
                                        currentCode++;
                                        break;
                                    case "highlight":
                                        currentHighlight++;
                                        break;
                                    case "link":
                                        linkAnnotations.Add(item);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        // collect text
                        currentText.Add(markdownText[i]);
                    }


                    // ending
                    if (currentText.Count > 0)
                    {
                        ixEnd = ixStart + currentText.Count;
                        // unclosed or last tag
                        this.ApplyColoring(spannable, ixStart, ixEnd, (currentCode > 0), (currentHighlight > 0));
                        this.ApplyIndents(spannable, ixStart, ixEnd, bodyIndent, forceCode, false);
                        this.ApplyFont(spannable, ixStart, ixEnd, fontSizeSp, (currentBold > 0), (currentItalic > 0), (currentUnderline > 0));
                        this.ApplyAlignment(spannable, ixStart, ixEnd, forceCenter);

                    }

                    spannable.EnsureEmojis(this.Context, (int)fontSizeSp);
                    return new MarkdownData(spannable, linkAnnotations);
                }
            });
        }

        protected void ApplyFont(SpannableString spannable, int ixStart, int ixEnd, float fontSizeSp, bool bold, bool italic, bool underline)
        {
            base.ExecuteMethod("ApplyFont", delegate ()
            {
                if (underline)
                {
                    spannable.SetRangeDecoration(ixStart, ixEnd, true);
                }
                if (bold && italic)
                {
                    spannable.SetRangeFont(ixStart, ixEnd, this.FontBoldItalic, fontSizeSp);
                }
                else if (bold)
                {
                    spannable.SetRangeFont(ixStart, ixEnd, this.FontBold, fontSizeSp);
                }
                else if (italic)
                {
                    spannable.SetRangeFont(ixStart, ixEnd, this.FontItalic, fontSizeSp);
                }
            });

        }
        protected void ApplyIndents(SpannableString spannable, int ixStart, int ixEnd, int bodyIndent, bool pureCode, bool blockQuote)
        {
            base.ExecuteMethod("ApplyIndents", delegate ()
            {
                if (pureCode) // code trumps all
                {
                    spannable.SetRangeIndent(ixStart, ixEnd, 10, 10);
                }
                else if (blockQuote)
                {
                    spannable.SetRangeIndent(ixStart, ixEnd, 1, 10);
                }
            });

        }
        protected void ApplyColoring(SpannableString spannable, int ixStart, int ixEnd, bool code, bool highlight)
        {
            base.ExecuteMethod("ApplyColoring", delegate ()
            {
                if (code) // code trumps all
                {
                    spannable.SetRangeColor(ixStart, ixEnd, StencilPreferences.COLOR_Markdown_Code_Background.ConvertHexToColor(), StencilPreferences.COLOR_Markdown_Code_Foreground.ConvertHexToColor());
                }
                else if (highlight)
                {
                    spannable.SetRangeColor(ixStart, ixEnd, StencilPreferences.COLOR_Markdown_Highlight_Background.ConvertHexToColor(), StencilPreferences.COLOR_Markdown_Highlight_Foreground.ConvertHexToColor());
                }
            });

        }
        protected void ApplyAlignment(SpannableString spannable, int ixStart, int ixEnd, bool center)
        {
            base.ExecuteMethod("ApplyAlignment", delegate ()
            {
                if (center)
                {
                    spannable.SetRangeAlignment(ixStart, ixEnd, Android.Text.Layout.Alignment.AlignCenter);
                }

            });
        }

        #endregion
    }
}