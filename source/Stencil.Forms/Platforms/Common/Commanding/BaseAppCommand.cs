using Stencil.Forms.Views;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Stencil.Forms.Commanding
{
    public abstract class BaseAppCommand<TAPI> : DynamicCommand<TAPI>, IAppCommand
        where TAPI : StencilAPI
    {
        public BaseAppCommand(TAPI api, string trackPrefix)
            : base(api, trackPrefix)
        {

        }

        public abstract bool AlertErrors { get; }
        public abstract Task<string> CanExecuteAsync(ICommandScope commandScope, IDataViewModel dataViewModel);

        public abstract Task<bool> ExecuteAsync(ICommandScope commandScope, object commandParameter, IDataViewModel dataViewModel);
    }
}
