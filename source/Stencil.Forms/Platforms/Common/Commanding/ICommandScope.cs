using Stencil.Forms.Presentation.Menus;
using System.Collections.Concurrent;

namespace Stencil.Forms.Commanding
{
    public interface ICommandScope
    {
        bool AlertErrors { get; }
        ICommandProcessor CommandProcessor { get; }
        ConcurrentDictionary<string, ConcurrentDictionary<string, ICommandField>> CommandData { get; }
        IMenuEntry TargetMenuEntry { get; }
        void RegisterCommandField(ICommandField commandField);
    }
}
