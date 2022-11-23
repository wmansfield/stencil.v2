using Android.Graphics.Drawables;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Controls.Platform;
using Stencil.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui.Handlers
{
    public partial class StencilCollectionViewHandler
    {
        static partial void ConnectHandler(CollectionViewHandler handler, StencilCollectionView editor)
        {
            CoreUtility.ExecuteMethod("StencilEditorHandler.ConnectHandler", delegate ()
            {
                AndroidX.RecyclerView.Widget.RecyclerView platformView = handler.PlatformView;
                if(platformView == null)
                {
                    return;
                }

                if(editor.SuppressOverScroll)
                {
                    platformView.OverScrollMode = Android.Views.OverScrollMode.Never;
                }
                else
                {
                    platformView.OverScrollMode = Android.Views.OverScrollMode.Always;
                }
            });
        }
    }
}
