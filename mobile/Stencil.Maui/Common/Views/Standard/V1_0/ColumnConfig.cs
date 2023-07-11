using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Stencil.Common.Views;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public class ColumnConfig : PropertyClass
    {
        public ColumnConfig()
        {
            this.ColumnWidth = "*";
            this.HorizontalOptions = LayoutOptions.Center;
            this.VerticalOptions = LayoutOptions.Center;
            this.Margin = new ThicknessInfo(0);
        }
        private LayoutOptions _horizontalOptions;
        public LayoutOptions HorizontalOptions
        {
            get { return _horizontalOptions; }
            set { SetProperty(ref _horizontalOptions, value); }
        }

        private LayoutOptions _verticalOptions;
        public LayoutOptions VerticalOptions
        {
            get { return _verticalOptions; }
            set { SetProperty(ref _verticalOptions, value); }
        }

        private ThicknessInfo _margin;
        public ThicknessInfo Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        private string _columnWidth;
        public string ColumnWidth
        {
            get { return _columnWidth; }
            set 
            {
                if (SetProperty(ref _columnWidth, value))
                {
                    OnPropertyChanged(nameof(ColumnGridLength));
                }
            }
        }

        private GridLength _columnGridLength;
        private string _columnGridLengthSource;
        public GridLength ColumnGridLength
        {
            get 
            {
                if (this.ColumnWidth == "*" || this.ColumnWidth == GridLength.Star.ToString())
                {
                    return GridLength.Star;
                }
                else
                {
                    if(_columnGridLengthSource != this.ColumnWidth)
                    {
                        _columnGridLengthSource = this.ColumnWidth;
                        double.TryParse(this.ColumnWidth, out double parsed);
                        _columnGridLength = new GridLength(parsed);
                    }
                    return _columnGridLength;
                }
            }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

    }
}
