using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Forms
{
    public class CenterOffsetExpression : IMarkupExtension<Constraint>
	{
		public CenterOffsetExpression()
		{
			this.MaxFactor = 2;
		}

        public CenterOffsetType Type { get; set; }

        public double MaxFactor { get; set; }

        public string ParentElementName { get; set; }
		public string ChildElementName { get; set; }


		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		{
			return (this as IMarkupExtension<Constraint>).ProvideValue(serviceProvider);
		}

		public Constraint ProvideValue(IServiceProvider serviceProvider)
		{
			if (string.IsNullOrEmpty(this.ParentElementName))
			{
				return null;
			}
			if (string.IsNullOrEmpty(this.ChildElementName))
			{
				return null;
			}

			IReferenceProvider referenceProvider = serviceProvider.GetService<IReferenceProvider>();
			if (referenceProvider == null)
			{
				return null;
			}

			View parentView = (View)referenceProvider.FindByName(this.ParentElementName);
			View childView = (View)referenceProvider.FindByName(this.ChildElementName);
			if (parentView == null || childView == null)
			{
				return null;
			}

            switch (this.Type)
            {
                case CenterOffsetType.Width:
					return Constraint.RelativeToView(parentView, delegate (RelativeLayout layout, View view)
					{
						double childValue = childView.Measure(parentView.Width * this.MaxFactor, parentView.Height, MeasureFlags.IncludeMargins).Request.Width;
						return childValue;
					});
                case CenterOffsetType.Height:
					return Constraint.RelativeToView(parentView, delegate (RelativeLayout layout, View view)
					{
						double childValue = childView.Measure(parentView.Width, parentView.Height * this.MaxFactor, MeasureFlags.IncludeMargins).Request.Height;
						return childValue;
					});
				case CenterOffsetType.Y:
					return Constraint.RelativeToView(parentView, delegate (RelativeLayout layout, View view)
					{
						double childValue = childView.Measure(parentView.Width, parentView.Height * this.MaxFactor, MeasureFlags.None).Request.Height;
						double result = 0;
						if (childValue > parentView.Height)
						{
							result = -((childValue - parentView.Height) / 2);
						}
						else if (childValue < parentView.Height)
						{
							result = ((parentView.Height - childValue) / 2);
						}
						result += childView.Margin.Top + childView.Margin.Bottom;
						return result;
					});
				default:
				case CenterOffsetType.X:
					return Constraint.RelativeToView(parentView, delegate (RelativeLayout layout, View view)
					{
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

						return result;
					});
            }
		}
	}
}