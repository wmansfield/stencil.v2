using Stencil.Maui.Views;
using System.Threading.Tasks;

namespace Stencil.Maui.Commanding
{
    public abstract class BaseDataCommand<TAPI> : DynamicCommand<TAPI>, IDataCommand
        where TAPI : StencilAPI
    {
        public BaseDataCommand(TAPI api, string trackPrefix)
            : base(api, trackPrefix)
        {

        }

        public abstract bool AlertErrors { get; }
        public abstract Task<string> CanExecuteAsync(ICommandScope commandScope, IDataViewModel dataViewModel);

        public abstract Task<object> ExecuteAsync(ICommandScope commandScope, object commandParameter, IDataViewModel dataViewModel);


    }
}
