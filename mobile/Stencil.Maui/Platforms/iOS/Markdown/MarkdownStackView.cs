using CoreGraphics;
using Foundation;
using Stencil.Maui;
using Stencil.Maui.iOS.Markdown;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;
using Stencil.Maui.iOS;

namespace Stencil.Maui.iOS.Markdown
{
    public class MarkdownStackView : UIStackView
    {
        public MarkdownStackView(string textColor)
            : base()
        {
            if (string.IsNullOrWhiteSpace(textColor))
            {
                textColor = "#000000";
            }
            this.Opaque = false;
            this.ContentMode = UIViewContentMode.ScaleToFill;
            this.Axis = UILayoutConstraintAxis.Vertical;
            this.TextColor = textColor;
        }
        
        #region Static Methods

        private const int HEADER_EXTRA_FONT_SIZE = 2;
        private const string BULLET_NUMBER_FORMAT = "{0}. ";
        private const string BULLET_PREFIX = "- ";
        private const string BLOCK_QUOTE_PREFIX = "“ ";
        private const string DIVIDER_TEXT = "----";
        private const int DIVIDER_FONT_SIZE = 10;
        private const int BULLET_NUMBER_BODY_INDENT = 15;
        private const int BULLET_DASH_BODY_INDENT = 10;
        private const int BLOCKQUOTE_BODY_INDENT = 10;
        public const int DEFAULT_IMAGE_HEIGHT = 270;
        public const int MAX_IMAGE_HEIGHT = 400;
        public const int MIN_IMAGE_HEIGHT = 20;

        public static float CalculateAndCacheCellHeight(IMarkdownHost host, CacheModel cacheModel, int width, List<MarkdownSection> sections, float emptyHeight, string textColor)
        {
            return CoreUtility.ExecuteFunction("MarkdownStackView.CalculateAndCacheCellHeight", delegate ()
            {
                float height = 0f;

                if (!cacheModel.TagExists("totalHeight"))
                {
                    if (sections == null || sections.Count == 0) // backwards compat, didnt support markdown
                    {
                        cacheModel.ui_text = null;
                        cacheModel.ui_height = height;
                    }
                    else
                    {
                        foreach (MarkdownSection section in sections)
                        {
                            if (!section.ui_height.HasValue)
                            {
                                switch (section.kind)
                                {
                                    case MarkdownSectionKind.header:
                                        section.ui_height = MarkdownStackView.CalculateAndCacheString(section, string.Empty, width, host.FontSize + HEADER_EXTRA_FONT_SIZE, 0, true, false, false, textColor);
                                        break;
                                    case MarkdownSectionKind.text:
                                        section.ui_height = MarkdownStackView.CalculateAndCacheString(section, string.Empty, width, host.FontSize, 0, false, false, false, textColor);
                                        break;
                                    case MarkdownSectionKind.bullet_number:
                                        float bulletHeight = 0;
                                        int number = 0;
                                        foreach (AnnotatedTextItem item in section.items)
                                        {
                                            number++;
                                            bulletHeight += MarkdownStackView.CalculateAndCacheString(item, string.Format(BULLET_NUMBER_FORMAT, number), width, host.FontSize, BULLET_NUMBER_BODY_INDENT, textColor);
                                        }
                                        section.ui_height = bulletHeight;
                                        break;
                                    case MarkdownSectionKind.bullet_text:
                                        float bulletTextHeight = 0;
                                        foreach (AnnotatedTextItem item in section.items)
                                        {
                                            bulletTextHeight += MarkdownStackView.CalculateAndCacheString(item, BULLET_PREFIX, width, host.FontSize, BULLET_DASH_BODY_INDENT, textColor);
                                        }
                                        section.ui_height = bulletTextHeight;
                                        break;
                                    case MarkdownSectionKind.code_block:

                                        section.ui_height = MarkdownStackView.CalculateAndCacheString(section, string.Empty, width, host.FontSize, 0, false, true, false, textColor);
                                        break;
                                    case MarkdownSectionKind.block_quote:
                                        section.ui_height = MarkdownStackView.CalculateAndCacheString(section, BLOCK_QUOTE_PREFIX, width, host.FontSize, BLOCKQUOTE_BODY_INDENT, false, false, false, textColor);
                                        break;
                                    case MarkdownSectionKind.image:
                                        section.ui_height = MarkdownStackView.CalculateAndCacheMedia(section, width);
                                        break;
                                    case MarkdownSectionKind.video:
                                        section.ui_height = MarkdownStackView.CalculateAndCacheMedia(section, width);
                                        break;
                                    case MarkdownSectionKind.divider:
                                        if (!host.SuppressDivider)
                                        {
                                            section.text = DIVIDER_TEXT;
                                            section.ui_height = MarkdownStackView.CalculateAndCacheString(section, string.Empty, width, DIVIDER_FONT_SIZE, 0, false, false, true, textColor);
                                        }

                                        break;
                                    default:
                                        section.ui_height = 0;
                                        break;
                                }
                            }
                            height += section.ui_height.GetValueOrDefault();
                        }

                        cacheModel.ui_height = height;
                    }
                    cacheModel.TagSet("totalHeight", height.ToString());
                }
                else
                {
                    height = cacheModel.TagGetAsInt("totalHeight", 30);
                }
                return height;
            });
        }

        #endregion

        #region Properties

        public IMarkdownHost Host { get; set; }
        public CacheModel CacheModel { get; set; }

        public string TextColor { get; set; }

        #endregion

        #region Public Methods

        public void ClearText()
        {
            CoreUtility.ExecuteMethod("ClearText", delegate ()
            {
                List<UIView> views = this.ArrangedSubviews.ToList();
                foreach (UIView view in views)
                {
                    this.RemoveArrangedSubview(view);
                    NSLayoutConstraint.DeactivateConstraints(view.Constraints);
                    view.RemoveFromSuperview();
                    view.Dispose();
                }
            });
        }
        public void SetText(IMarkdownHost host, CacheModel cacheModel, int width, List<MarkdownSection> sections)
        {
            CoreUtility.ExecuteMethod("SetText", delegate ()
            {
                if (this.Host == host && this.CacheModel == cacheModel && cacheModel.MarkdownHasGenerated())
                {
                    if (this.ArrangedSubviews.FirstOrDefault() != null)
                    {
                        CoreUtility.Logger.LogDebug("Rebind detected, skipping");
                        return;
                    }
                }
                cacheModel.MarkdownSetGenerated(true);
                this.Host = host;
                this.CacheModel = cacheModel;

                float height = MarkdownStackView.CalculateAndCacheCellHeight(host, cacheModel, width, sections, 0, this.TextColor); // should do nothing in tables, but may be missing for standalone

                this.Alignment = UIStackViewAlignment.Fill;

                this.ClearText();

                List<UIView> viewsToAdd = new List<UIView>();

                if (sections == null || sections.Count == 0)
                {
                    MarkDownLabel label = this.GenerateLabel(cacheModel.ui_height, host.FontSize);
                    if (cacheModel.ui_text == null)
                    {
                        cacheModel.ui_text = new NSMutableAttributedString();
                    }
                    label.AttributedText = cacheModel.ui_text;
                    viewsToAdd.Add(label);
                }
                else
                {
                    foreach (MarkdownSection section in sections)
                    {
                        switch (section.kind)
                        {
                            case MarkdownSectionKind.header:
                                MarkDownLabel headerLabel = this.GenerateLabel(section.ui_height, host.FontSize + HEADER_EXTRA_FONT_SIZE);
                                headerLabel.AttributedText = (NSMutableAttributedString)section.ui_text;
                                headerLabel.WireUpLinks(section.ui_links, host.LinkTapped);
                                viewsToAdd.Add(headerLabel);
                                break;
                            case MarkdownSectionKind.text:
                                MarkDownLabel label = this.GenerateLabel(section.ui_height, host.FontSize);
                                label.AttributedText = (NSMutableAttributedString)section.ui_text;
                                label.WireUpLinks(section.ui_links, host.LinkTapped);
                                viewsToAdd.Add(label);
                                break;
                            case MarkdownSectionKind.bullet_number:
                                int number = 0;
                                foreach (AnnotatedTextItem item in section.items)
                                {
                                    number++;
                                    MarkDownLabel bulletLabel = this.GenerateLabel(item.ui_height, host.FontSize);
                                    bulletLabel.AttributedText = (NSMutableAttributedString)item.ui_text;
                                    bulletLabel.WireUpLinks(item.ui_links, host.LinkTapped);

                                    viewsToAdd.Add(bulletLabel);
                                }
                                break;
                            case MarkdownSectionKind.bullet_text:
                                foreach (AnnotatedTextItem item in section.items)
                                {
                                    MarkDownLabel bulletLabel = this.GenerateLabel(item.ui_height, host.FontSize);
                                    bulletLabel.AttributedText = (NSMutableAttributedString)item.ui_text;
                                    bulletLabel.WireUpLinks(item.ui_links, host.LinkTapped);
                                    viewsToAdd.Add(bulletLabel);
                                }
                                break;
                            case MarkdownSectionKind.code_block:
                                MarkDownLabel codeLabel = this.GenerateLabel(section.ui_height, host.FontSize);
                                codeLabel.BackgroundColor = StencilPreferences.COLOR_Markdown_Code_Background.ConvertHexToColor();
                                codeLabel.AttributedText = (NSMutableAttributedString)section.ui_text;
                                codeLabel.WireUpLinks(section.ui_links, host.LinkTapped);
                                viewsToAdd.Add(codeLabel);
                                break;
                            case MarkdownSectionKind.block_quote:
                                MarkDownLabel blockLabel = this.GenerateLabel(section.ui_height, host.FontSize);
                                blockLabel.AttributedText = (NSMutableAttributedString)section.ui_text;
                                blockLabel.WireUpLinks(section.ui_links, host.LinkTapped);
                                viewsToAdd.Add(blockLabel);
                                break;
                            case MarkdownSectionKind.image:
                            case MarkdownSectionKind.video:
                                MarkDownMediaView mediaView = new MarkDownMediaView();
                                mediaView.SetMedia(host, (int)this.Bounds.Width, section);
                                viewsToAdd.Add(mediaView);
                                break;
                            case MarkdownSectionKind.divider:
                                if (!host.SuppressDivider)
                                {
                                    MarkDownLabel dividerLabel = this.GenerateLabel(section.ui_height, host.FontSize);
                                    dividerLabel.AttributedText = (NSMutableAttributedString)section.ui_text;
                                    dividerLabel.WireUpLinks(section.ui_links, host.LinkTapped);
                                    viewsToAdd.Add(dividerLabel);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                foreach (UIView view in viewsToAdd)
                {
                    this.AddArrangedSubview(view);
                }
            });
        }

        #endregion

        #region Protected Methods

        protected MarkDownLabel GenerateLabel(float? expectedHeight, float fontSize)
        {
            MarkDownLabel label = new MarkDownLabel(this.Host.LinkTapped);
            label.Opaque = false;
            //label.Lines = 0;
            label.ExpectedSize = new CGSize(this.Frame.Width, expectedHeight.GetValueOrDefault());
            label.Frame = new CGRect(0, 0, this.Frame.Width, expectedHeight.GetValueOrDefault());
            label.InsetsLayoutMarginsFromSafeArea = false;
            label.DirectionalLayoutMargins = new NSDirectionalEdgeInsets(0, 0, 0, 0);
            label.LayoutMargins = new UIEdgeInsets(0, 0, 0, 0);
            //label.LineBreakMode = UILineBreakMode.WordWrap;
            label.BackgroundColor = UIColor.Clear;
            label.TextColor = this.TextColor.ConvertHexToColor();
            label.Font = UIFont.FromName(StencilPreferences.FONT_REGULAR, fontSize);
            label.UserInteractionEnabled = true;


            label.AddGestureRecognizer(new UITapGestureRecognizer(OnLabelTapped) { NumberOfTapsRequired = 1 });
            label.UserInteractionEnabled = true;

            return label;
        }

        #endregion

        #region Generation Methods

        private static float CalculateAndCacheMedia(MarkdownSection section, int width)
        {
            return CoreUtility.ExecuteFunction("MarkdownStackView.CalculateAndCacheMedia", delegate ()
            {
                if (!section.ui_height.HasValue && section.asset != null)
                {
                    float imageSize = DEFAULT_IMAGE_HEIGHT;
                    Size size = Size.Empty;
                    if (CoreUtility.TryParseDimensions(section.asset.dimensions, out size))
                    {
                        int imageHeight = (int)(width.SizingScaleFix() * (float)size.Height / (float)size.Width);// W/H = w/?
                        if (imageHeight < MIN_IMAGE_HEIGHT)
                        {
                            imageHeight = MIN_IMAGE_HEIGHT;
                        }
                        if (imageHeight > MAX_IMAGE_HEIGHT)
                        {
                            imageHeight = MAX_IMAGE_HEIGHT;
                        }

                        imageSize = imageHeight + 10; // 5+5 for margin
                    }
                    section.ui_height = imageSize;
                }
                else if (section.ui_data != null)
                {
                    UIImage image = section.ui_data as UIImage;
                    if (image != null)
                    {
                        int imageHeight = (int)(width.SizingScaleFix() * image.Size.Height / image.Size.Width);// W/H = w/?
                        if (imageHeight < MIN_IMAGE_HEIGHT)
                        {
                            imageHeight = MIN_IMAGE_HEIGHT;
                        }
                        if (imageHeight > MAX_IMAGE_HEIGHT)
                        {
                            imageHeight = MAX_IMAGE_HEIGHT;
                        }

                        section.ui_height = imageHeight + 10; // 5+5 for margin
                    }
                }

                return section.ui_height.GetValueOrDefault();
            });
        }
        private static float CalculateAndCacheStringUnstyled(MarkdownSection section, int width, string text, float fontSize, float emptyHeight, string textColor)
        {
            return CoreUtility.ExecuteFunction("MarkdownStackView.CalculateAndCacheStringUnstyled", delegate ()
            {
                NSMutableAttributedString attributedString = (NSMutableAttributedString)section.ui_text;

                if (attributedString == null)
                {
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        attributedString = new NSMutableAttributedString(string.Empty);
                        section.ui_text = attributedString;
                        section.ui_height = emptyHeight;
                    }
                    else
                    {
                        attributedString = MarkdownStackView.GenerateTextRegular(text, fontSize, textColor);
                        section.ui_text = attributedString;

                        CGRect boudingRect = attributedString.GetBoundingRect(new CGSize(width.SizingScaleFix(), 5000), NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, new NSStringDrawingContext());
                        section.ui_height = _Extensions.ClampToLineHeights(boudingRect, fontSize);
                    }

                }

                return section.ui_height.GetValueOrDefault();
            });
        }
        private static float CalculateAndCacheString(MarkdownSection section, string sectionPrefix, int width, float fontSize, int bodyIndent, bool forceBold, bool forceCode, bool forceCenter, string textColor)
        {
            return CoreUtility.ExecuteFunction("MarkdownStackView.CalculateAndCacheString", delegate ()
            {
                if (sectionPrefix == null)
                {
                    sectionPrefix = string.Empty;
                }
                NSMutableAttributedString attributedString = (NSMutableAttributedString)section.ui_text;

                if (attributedString == null)
                {
                    MarkdownData data = MarkdownStackView.GenerateMarkdownText(sectionPrefix, section.text, section.annotations, fontSize, bodyIndent, forceBold, forceCode, forceCenter, textColor);
                    attributedString = data.Text;
                    section.ui_text = data.Text;
                    section.ui_links = data.Links;

                    CGRect boudingRect = attributedString.GetBoundingRect(new CGSize(width.SizingScaleFix(), 5000), NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, new NSStringDrawingContext());
                    section.ui_height = _Extensions.ClampToLineHeights(boudingRect, fontSize);
                }

                return section.ui_height.GetValueOrDefault();
            });
        }
        private static float CalculateAndCacheString(AnnotatedTextItem annotatedText, string prefix, int width, float fontSize, int bodyIndent, string textColor)
        {
            return CoreUtility.ExecuteFunction("MarkdownStackView.CalculateAndCacheString", delegate ()
            {
                NSMutableAttributedString attributedString = (NSMutableAttributedString)annotatedText.ui_text;
                if (attributedString == null)
                {
                    MarkdownData data = MarkdownStackView.GenerateMarkdownText(prefix, annotatedText.text, annotatedText.annotations, fontSize, bodyIndent, false, false, false, textColor);
                    attributedString = data.Text;
                    if (annotatedText != null)
                    {
                        annotatedText.ui_text = data.Text;
                        annotatedText.ui_links = data.Links;

                        CGRect boudingRect = attributedString.GetBoundingRect(new CGSize(width.SizingScaleFix(), 5000), NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, new NSStringDrawingContext());
                        annotatedText.ui_height = _Extensions.ClampToLineHeights(boudingRect, fontSize);
                    }
                }

                return annotatedText.ui_height.GetValueOrDefault();
            });
        }

        protected static NSMutableAttributedString GenerateTextRegular(string text, float fontSize, string textColor)
        {
            return new NSMutableAttributedString(text, GenerateDefaultAttributes(StencilPreferences.FONT_REGULAR, fontSize, textColor));
        }

        protected static UIStringAttributes GenerateDefaultAttributes(string fontName, float fontSize, string textColor)
        {
            return new UIStringAttributes()
            {
                Font = UIFont.FromName(fontName, fontSize),
                ForegroundColor = textColor.ConvertHexToColor(),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    Alignment = UITextAlignment.Left,
                    LineSpacing = 0,
                    ParagraphSpacing = 0,
                    ParagraphSpacingBefore = 0,
                    LineBreakMode = UILineBreakMode.WordWrap,
                }
            };
        }

        protected static MarkdownData GenerateMarkdownText(string prefix, string markdownText, List<TextAnnotation> annotations, float fontSize, int bodyIndent, bool forceBold, bool forceCode, bool forceCenter, string textColor)
        {
            if (annotations == null || annotations.Count == 0)
            {
                UIStringAttributes attributes = GenerateDefaultAttributes(StencilPreferences.FONT_REGULAR, fontSize, textColor);

                MarkdownStackView.ApplyColoring(attributes, forceCode, false, false, textColor);
                MarkdownStackView.ApplyIndents(attributes, bodyIndent, forceCode, false);
                MarkdownStackView.ApplyFont(attributes, fontSize, forceBold, false, false);

                if (forceCenter)
                {
                    attributes.ParagraphStyle.Alignment = UITextAlignment.Center;
                }

                string allText = string.Format("{0}{1}", prefix, markdownText);


                NSMutableAttributedString text = new NSMutableAttributedString(allText.TrimSafe(), attributes);
                return new MarkdownData(text, new List<TextAnnotation>());
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


                NSMutableAttributedString result = null;

                int currentBold = forceBold ? 100 : 0;
                int currentItalic = 0;
                int currentUnderline = 0;
                int currentCode = forceCode ? 100 : 0;
                int currentHighlight = 0;
                int currentLink = 0;

                UIStringAttributes currentAttributes = GenerateDefaultAttributes(StencilPreferences.FONT_REGULAR, fontSize, textColor);
                currentAttributes.ParagraphStyle.HeadIndent = bodyIndent;
                if (forceCenter)
                {
                    currentAttributes.ParagraphStyle.Alignment = UITextAlignment.Center;
                }

                MarkdownStackView.ApplyColoring(currentAttributes, (currentCode > 0), (currentHighlight > 0), (currentLink > 0), textColor);
                MarkdownStackView.ApplyIndents(currentAttributes, bodyIndent, forceCode, false);
                MarkdownStackView.ApplyFont(currentAttributes, fontSize, (currentBold > 0), (currentItalic > 0), (currentUnderline > 0));

                List<TextAnnotation> linkAnnotations = new List<TextAnnotation>();

                List<char> currentText = new List<char>(markdownText.Length);
                if (prefix != null)
                {
                    currentText.AddRange(prefix.ToCharArray());
                }
                for (int i = 0; i < markdownText.Length; i++)
                {
                    if (endAnnotations.ContainsKey(i))
                    {
                        NSMutableAttributedString recentString = null;
                        if (currentText.Count > 0)
                        {
                            // close current
                            recentString = new NSMutableAttributedString(new string(currentText.ToArray()), currentAttributes);
                            if (result != null)
                            {
                                result.Append(recentString);
                            }
                            else
                            {
                                result = recentString;
                            }
                            currentText.Clear();
                        }

                        // set up the next
                        List<TextAnnotation> matchingAnnotations = endAnnotations[i].ToList(); // outer ones first
                        foreach (TextAnnotation item in matchingAnnotations)
                        {
                            switch (item.type)
                            {
                                case "link":
                                    currentLink = Math.Max(0, currentLink - 1);
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


                        MarkdownStackView.ApplyColoring(currentAttributes, (currentCode > 0), (currentHighlight > 0), (currentLink > 0), textColor);
                        MarkdownStackView.ApplyIndents(currentAttributes, bodyIndent, forceCode, false);
                        MarkdownStackView.ApplyFont(currentAttributes, fontSize, (currentBold > 0), (currentItalic > 0), (currentUnderline > 0));
                    }

                    if (startAnnotations.ContainsKey(i))
                    {
                        if (currentText.Count > 0)
                        {
                            // close current
                            NSMutableAttributedString currentString = new NSMutableAttributedString(new string(currentText.ToArray()), currentAttributes);
                            if (result != null)
                            {
                                result.Append(currentString);
                            }
                            else
                            {
                                result = currentString;
                            }
                            currentText.Clear();
                        }

                        // start new styles
                        List<TextAnnotation> matchingAnnotations = startAnnotations[i].ToList();
                        string forcedLinkColor = string.Empty;

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
                                    currentLink++;
                                    linkAnnotations.Add(item);
                                    if (!string.IsNullOrWhiteSpace(item.color))
                                    {
                                        forcedLinkColor = item.color;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        MarkdownStackView.ApplyColoring(currentAttributes, (currentCode > 0), (currentHighlight > 0), (currentLink > 0), textColor, forcedLinkColor);
                        MarkdownStackView.ApplyIndents(currentAttributes, bodyIndent, forceCode, false);
                        MarkdownStackView.ApplyFont(currentAttributes, fontSize, (currentBold > 0), (currentItalic > 0), (currentUnderline > 0));
                    }


                    // collect text
                    currentText.Add(markdownText[i]);
                }


                // ending
                if (currentText.Count > 0)
                {
                    if (currentAttributes == null)
                    {
                        currentAttributes = GenerateDefaultAttributes(StencilPreferences.FONT_REGULAR, fontSize, textColor);

                        if (forceCenter)
                        {
                            currentAttributes.ParagraphStyle.Alignment = UITextAlignment.Center;
                        }
                    }

                    NSMutableAttributedString currentString = new NSMutableAttributedString(new string(currentText.ToArray()), currentAttributes);
                    if (result != null)
                    {
                        result.Append(currentString);
                    }
                    else
                    {
                        result = currentString;
                    }
                }

                if (result == null)
                {
                    result = GenerateTextRegular("", fontSize, textColor);
                }
                return new MarkdownData(result, linkAnnotations);
            }
        }

        protected static void ApplyIndents(UIStringAttributes currentAttributes, int bodyIndent, bool pureCode, bool blockQuote)
        {
            if (pureCode) // code trumps all
            {
                currentAttributes.ParagraphStyle.FirstLineHeadIndent = bodyIndent + 10;
                currentAttributes.ParagraphStyle.HeadIndent = bodyIndent + 10;
            }
            else if (blockQuote)
            {
                currentAttributes.ParagraphStyle.FirstLineHeadIndent = 0;
                currentAttributes.ParagraphStyle.HeadIndent = bodyIndent + 10;
            }
            else
            {
                currentAttributes.ParagraphStyle.FirstLineHeadIndent = 0;
                currentAttributes.ParagraphStyle.HeadIndent = 0;
            }
        }
        protected static void ApplyColoring(UIStringAttributes currentAttributes, bool code, bool highlight, bool link, string textColor, string linkColor = null)
        {
            if (code) // code trumps all
            {
                currentAttributes.BackgroundColor = StencilPreferences.COLOR_Markdown_Code_Background.ConvertHexToColor();
                currentAttributes.ForegroundColor = StencilPreferences.COLOR_Markdown_Code_Foreground.ConvertHexToColor();
            }
            else if (highlight)
            {
                currentAttributes.BackgroundColor = StencilPreferences.COLOR_Markdown_Highlight_Background.ConvertHexToColor();
                currentAttributes.ForegroundColor = StencilPreferences.COLOR_Markdown_Highlight_Foreground.ConvertHexToColor();
            }
            else if (link)
            {
                currentAttributes.BackgroundColor = UIColor.Clear;
                currentAttributes.ForegroundColor = string.IsNullOrWhiteSpace(linkColor) ? StencilPreferences.COLOR_Markdown_Link_Foreground.ConvertHexToColor() : linkColor.ConvertHexToColor();
            }
            else
            {
                currentAttributes.BackgroundColor = UIColor.Clear;
                currentAttributes.ForegroundColor = textColor.ConvertHexToColor();
            }
        }
        protected static void ApplyFont(UIStringAttributes currentAttributes, float fontSize, bool bold, bool italic, bool underline)
        {
            if (underline)
            {
                currentAttributes.UnderlineStyle = NSUnderlineStyle.Single;
            }
            else
            {
                currentAttributes.UnderlineStyle = NSUnderlineStyle.None;
            }

            if (bold && italic)
            {
                currentAttributes.Font = UIFont.FromName(StencilPreferences.FONT_BOLD_ITALIC, fontSize);
            }
            else if (bold)
            {
                currentAttributes.Font = UIFont.FromName(StencilPreferences.FONT_BOLD, fontSize);
            }
            else if (italic)
            {
                currentAttributes.Font = UIFont.FromName(StencilPreferences.FONT_ITALIC, fontSize);
            }
            else
            {
                currentAttributes.Font = UIFont.FromName(StencilPreferences.FONT_REGULAR, fontSize);
            }
        }

        #endregion

        protected void OnLabelTapped()
        {
            CoreUtility.ExecuteMethod("OnLabelTapped", delegate ()
            {
                this.Host?.AnythingTapped();
            });
        }


        public class VerticalFillStackViewView : UIView
        {
            public VerticalFillStackViewView()
            {
                this.SetContentHuggingPriority(249, UILayoutConstraintAxis.Vertical);
            }
            public override CGSize IntrinsicContentSize
            {
                get
                {
                    return new CGSize(1, 0);
                }
            }
        }
    }
}