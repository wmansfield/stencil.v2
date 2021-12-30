using Stencil.Native.Screens;
using Stencil.Native.Views;
using Stencil.Native.Views.Standard;
using System.Threading.Tasks;

namespace Stencil.Native.Commanding.Commands
{
    public class NavigateRootCommand : BaseNavigationCommand<StencilAPI>
    {
        public NavigateRootCommand()
            : base(StencilAPI.Instance, nameof(NavigateRootCommand))
        {

        }
        public override bool AlertErrors
        {
            get
            {
                return false;
            }
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
                NavigationData navigationData = this.ParseNavigationData<NavigationData>(commandParameter);

                IDataViewModel dataViewModel = await this.API.Screens.GenerateViewModelAsync(this.API.CommandProcessor, navigationData);
                StandardDataView dataView = new StandardDataView(dataViewModel);
                await this.API.Router.SetInitialViewAsync(dataView);
                return true;
            });
        }
    }
}
