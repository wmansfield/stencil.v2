using Stencil.Native.Presentation.Menus;
using System.Collections.Concurrent;

namespace Stencil.Native.Commanding
{
    public interface ICommandScope
    {
        StencilAPI API { get;}
        ICommandProcessor CommandProcessor { get; }
        ConcurrentDictionary<string, ConcurrentDictionary<string, ICommandField>> command_data { get; }
        IMenuEntry TargetMenuEntry { get; }
        void RegisterCommandField(ICommandField commandField);
    }
}
