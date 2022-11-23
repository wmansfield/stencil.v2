using Microsoft.Maui;
using Microsoft.Maui.Controls.Handlers.Items;
using Stencil.Maui.Controls;

namespace Stencil.Maui.Handlers
{
    public partial class StencilCollectionViewHandler
    {
        public static void Register()
        {
            CollectionViewHandler.Mapper.AppendToMapping(nameof(StencilCollectionViewHandler), (handler, view) =>
            {
                if(view is StencilCollectionView stencilCollectionView)
                {
                    ConnectHandler(handler, stencilCollectionView);
                }
            });
        }

        static partial void ConnectHandler(CollectionViewHandler handler, StencilCollectionView editor);
    }
}
