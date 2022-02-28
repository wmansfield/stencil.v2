using Foundation;
using Stencil.Forms.Effects;
using Stencil.Forms.iOS.Effects;
using Stencil.Forms.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(NoSelectionViewCellRenderer))]

namespace Stencil.Forms.iOS.Renderers
{
    public class NoSelectionViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            return CoreUtility.ExecuteFunction(nameof(GetCell), delegate ()
            {
                UITableViewCell result = base.GetCell(item, reusableCell, tv);

                result.SelectionStyle = UITableViewCellSelectionStyle.None;

                return result;
            });
            
        }
    }
}