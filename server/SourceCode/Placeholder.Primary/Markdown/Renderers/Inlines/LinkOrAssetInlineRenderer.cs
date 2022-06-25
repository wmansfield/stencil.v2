/*
The MIT License
Copyright (c) 2016 Social Haven

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
'Software'), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Markdown.Renderers.Inlines
{
    public class LinkOrAssetInlineRenderer<TSection> : StencilObjectRenderer<TSection, LinkInline>
        where TSection : class
    {
        private const string VIDEO_PREFIX = "video://";
        private const string IMAGE_PREFIX = "image://";
        protected override void Write(StencilRenderer<TSection> renderer, LinkInline linkInline)
        {
            string url = linkInline.GetDynamicUrl?.Invoke() ?? linkInline.Url;

            bool swallowContent = false;
            if (linkInline.IsImage)
            {
                if (url.StartsWith(VIDEO_PREFIX, StringComparison.OrdinalIgnoreCase))
                {
                    swallowContent = true;
                    if(url.Contains("."))
                    {
                        // presume url
                        string path = url.Substring(VIDEO_PREFIX.Length);

                        TSection section = renderer.Generator.GenerateVideoSection(null, path);
                        if (section != null)
                        {
                            renderer.Sections.Add(section);
                        }
                    }
                    else
                    {
                        // presume identifier
                        string identifier = url.Substring(VIDEO_PREFIX.Length);
                        TSection section = renderer.Generator.GenerateVideoSection(identifier);
                        if (section != null)
                        {
                            renderer.Sections.Add(section);
                        }
                    }
                }
                else if(url.StartsWith(IMAGE_PREFIX, StringComparison.OrdinalIgnoreCase))
                {
                    swallowContent = true;
                    if (url.Contains("."))
                    {
                        // presume url
                        string path = url.Substring(IMAGE_PREFIX.Length);
                        TSection section = renderer.Generator.GenerateImageSection(null, path);
                        if (section != null)
                        {
                            renderer.Sections.Add(section);
                        }
                    }
                    else
                    {
                        // presume identifier
                        string identifier = url.Substring(IMAGE_PREFIX.Length);
                        TSection section = renderer.Generator.GenerateImageSection(identifier);
                        if (section != null)
                        {
                            renderer.Sections.Add(section);
                        }
                    }
                }
            }


            // write link text
            string type = "link";
            string prefix = string.Empty;
            string suffix = string.Empty;
            string linkText = string.Empty;
            if (string.IsNullOrEmpty(linkInline.Title))
            {
                //[Text](https://website.com)
                linkText = string.Format("({0})", linkInline.Url);
                prefix = string.Format("<{0}>{2}</{0}><{1}>", StencilTags.TAG_ALINK, StencilTags.TAG_ACONTENT, linkInline.Url);
                suffix = string.Format("</{0}>", StencilTags.TAG_ACONTENT);
            }
            else
            {
                //[Text](https://website.com) NotReallyUsed)
                linkText = string.Format("({0} \"{1}\")", linkInline.Url, linkInline.Title);
                prefix = string.Format("<{0}>{4}</{0}><{1}>{3}</{1}><{2}>", StencilTags.TAG_ATITLE, StencilTags.TAG_ALINK, StencilTags.TAG_ACONTENT, linkInline.Url, linkInline.Title);
                suffix = string.Format("</{0}>", StencilTags.TAG_ACONTENT);
            }

            renderer.StartSpan(new SpanData() { type = type, target = url }, prefix);
            renderer.WriteChildren(linkInline);
            renderer.EndSpan(type, string.Format("{0}{1}", suffix, linkText), !swallowContent);
        }
    }
}
