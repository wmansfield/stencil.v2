using Stencil.Forms.Views;
using System.Threading.Tasks;

namespace Stencil.Forms.Commanding
{
    public interface ICommandProcessor
    {
        Task LinkTapped(string destination);
        Task<bool> ExecuteCommandAsync(ICommandScope commandScope, string commandName, object commandParameter, IDataViewModel dataViewModel);
        Task<object> ExecuteDataCommandAsync(ICommandScope commandScope, string commandName, object commandParameter, IDataViewModel dataViewModel);
    }
}
