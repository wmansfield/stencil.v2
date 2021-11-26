using System.Threading.Tasks;

namespace Stencil.Native.Commanding
{
    public interface ICommandProcessor
    {
        Task<bool> ExecuteCommandAsync(ICommandScope commandScope, string commandName, object commandParameter = null);
    }
}
