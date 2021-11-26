using Stencil.Native.Views;
using Stencil.Native.Views.Standard;
using System.Threading.Tasks;

namespace Stencil.Native.Commanding.Commands
{
    public class NavigateRootCommand : BaseAppCommand<StencilAPI>
    {
        public NavigateRootCommand()
            : base(StencilAPI.Instance, nameof(NavigateRootCommand))
        {

        }

        public override Task<string> CanExecuteAsync(ICommandScope commandScope)
        {
            return base.ExecuteFunction(nameof(CanExecuteAsync), delegate ()
            {
                return Task.FromResult((string)null);
            });
        }

        public override Task<bool> ExecuteAsync(ICommandScope commandScope, object commandParameter)
        {
            return base.ExecuteFunctionAsync(nameof(ExecuteAsync), async delegate()
            {
                IDataViewModel dataViewModel = await this.API.Screens.GenerateScreenAsync(this.API.CommandProcessor, $"{commandParameter}");
                StandardDataView dataView = new StandardDataView(dataViewModel);
                await this.API.Router.SetInitialViewAsync(dataView);
                return true;
            });
        }
    }
}
