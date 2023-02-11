using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Maui.Presentation.Routing;
using Stencil.Maui.Screens;
using Stencil.Maui.Views;
using Stencil.Maui.Views.Standard;
using System.Threading;
using System.Threading.Tasks;

namespace Stencil.Maui.Commanding.Commands
{
    public class NavigatePushCommand : BaseNavigationCommand<StencilAPI>
    {
        public NavigatePushCommand()
            : base(StencilAPI.Instance, nameof(NavigatePushCommand))
        {

        }
        public NavigatePushCommand(string trackPrefix)
            : base(StencilAPI.Instance, trackPrefix)
        {

        }

        public override bool AlertErrors
        {
            get
            {
                return false;
            }
        }

        public override Task<string> CanExecuteAsync(ICommandScope commandScope, IDataViewModel dataViewModel)
        {
            return base.ExecuteFunction(nameof(CanExecuteAsync), delegate ()
            {
                return Task.FromResult((string)null);
            });
        }

        public override Task<bool> ExecuteAsync(ICommandScope commandScope, object commandParameter, IDataViewModel dataViewModel, CancellationToken token = default)
        {
            return base.ExecuteFunctionAsync(nameof(ExecuteAsync), async delegate ()
            {
                NavigationData navigationData = this.ParseNavigationData<NavigationData>(commandParameter);

                IDataViewModel dataViewModel = await this.API.StencilScreens.GenerateViewModelAsync(this.API.CommandProcessor, navigationData);
                if (dataViewModel == null)
                {
                    return false;
                }
                else
                {
                    dataViewModel.NavigationData = navigationData; // force it

                    IRouterView dataView = this.GenerateView(dataViewModel);
                    await this.API.Router.PushViewAsync(dataView, commandScope.TargetMenuEntry);
                    return true;
                }
            });
        }

        protected virtual IRouterView GenerateView(IDataViewModel dataViewModel)
        {
            return base.ExecuteFunction(nameof(GenerateView), delegate ()
            {
                return new StandardDataView(dataViewModel);
            });
        }
        
    }
}
