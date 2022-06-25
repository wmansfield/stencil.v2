using Stencil.Common;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placeholder.Primary.UI
{
    public partial class WellKnownComponents
    {
        public class Image
        {
            public const string NAME = "image";
            public class Config
            {
                public Config()
                {
                    this.Width = -1;
                    this.Height = -1;
                    this.ImageWidth = -1;
                    this.ImageHeight = -1;
                }
                public string Source { get; set; }
                public int Width { get; set; }
                public int Height { get; set; }
                public int ImageWidth { get; set; }
                public int ImageHeight { get; set; }
                public string BackgroundColor { get; set; }
                public ThicknessInfo Padding { get; set; }


                public bool FullBleedHorizontal { get; set; }

                public string CommandName { get; set; }
                public string CommandParameter { get; set; }

            }
        }


    }
}
