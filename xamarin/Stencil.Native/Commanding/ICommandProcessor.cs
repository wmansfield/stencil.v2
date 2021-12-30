using System.Threading.Tasks;

namespace Stencil.Native.Commanding
{
    public interface ICommandProcessor
    {
        Task LinkTapped(string destination);
        Task<bool> ExecuteCommandAsync(ICommandScope commandScope, string commandName, object commandParameter = null);
        Task<object> ExecuteDataCommandAsync(ICommandScope commandScope, string commandName, object commandParameter = null);
    }
}
