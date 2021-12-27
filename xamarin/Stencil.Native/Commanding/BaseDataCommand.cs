using System.Threading.Tasks;

namespace Stencil.Native.Commanding
{
    public abstract class BaseDataCommand<TAPI> : DynamicCommand<TAPI>, IDataCommand
        where TAPI : StencilAPI
    {
        public BaseDataCommand(TAPI api, string trackPrefix)
            : base(api, trackPrefix)
        {

        }

        public abstract Task<string> CanExecuteAsync(ICommandScope commandScope);

        public abstract Task<object> ExecuteAsync(ICommandScope commandScope, object commandParameter);


    }
}
