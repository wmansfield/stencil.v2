using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Stencil.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui.Handlers
{
    public partial class StencilEditorHandler
    {
        public static void Register()
        {
            EditorHandler.Mapper.AppendToMapping(nameof(StencilEditorHandler), (handler, view) =>
            {
                if(view is StencilEditor stencilEditor)
                {
                    ConnectHandler(handler, stencilEditor);
                }
            });
        }

        static partial void ConnectHandler(IEditorHandler handler, StencilEditor editor);
    }
}
