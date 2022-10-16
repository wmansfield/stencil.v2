using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Stencil.Maui
{
    public class CenterPositionExpression : IMarkupExtension<Rect>
	{
		public CenterPositionExpression()
		{
			this.MaxFactor = 2;
		}

        public CenterOffsetType Type { get; set; }

        public double MaxFactor { get; set; }

        public string ParentElementName { get; set; }
		public string ChildElementName { get; set; }


		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		{
			return this.ProvideValue(serviceProvider);
		}

		public Rect ProvideValue(IServiceProvider serviceProvider)
		{
			if (string.IsNullOrEmpty(this.ParentElementName))
			{
				return Rect.Zero;
			}
			if (string.IsNullOrEmpty(this.ChildElementName))
			{
				return Rect.Zero;
			}

			IReferenceProvider referenceProvider = serviceProvider.GetService<IReferenceProvider>();
			if (referenceProvider == null)
			{
				return Rect.Zero;
			}

			View parentView = (View)referenceProvider.FindByName(this.ParentElementName);
			View childView = (View)referenceProvider.FindByName(this.ChildElementName);
			if (parentView == null || childView == null)
			{
				return Rect.Zero;
			}

            switch (this.Type)
            {
                case CenterOffsetType.Width:
                case CenterOffsetType.Height:
				case CenterOffsetType.Y:
				default:
				case CenterOffsetType.X:
                    double childValue = childView.Measure(parentView.Width * this.MaxFactor, parentView.Width, MeasureFlags.None).Request.Width;
                    double result = 0;
                    if (childValue > parentView.Width)
                    {
                        result = -((childValue - parentView.Width) / 2);
                    }
                    else if (childValue < parentView.Width)
                    {
                        result = ((parentView.Width - childValue) / 2);
                    }
                    result += childView.Margin.Left + childView.Margin.Right;

                    return new Rect(result, 0, childView.Width, childView.Height);
            }
		}

	}
}