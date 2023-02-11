using Stencil.Maui.Views;
using System.Threading;
using System.Threading.Tasks;

namespace Stencil.Maui.Commanding
{
    public interface ICommandProcessor
    {
        Task LinkTapped(string destination);

        bool IsDataCommand(string commandName);
        bool IsAppCommand(string commandName);

        Task<bool> ExecuteCommandAsync(ICommandScope commandScope, string commandName, object commandParameter, IDataViewModel dataViewModel, CancellationToken token = default);
        Task<object> ExecuteDataCommandAsync(ICommandScope commandScope, string commandName, object commandParameter, IDataViewModel dataViewModel, CancellationToken token = default);
    }
}
