using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Forms.Presentation.Routing;
using Stencil.Forms.Screens;
using Stencil.Forms.Views;
using Stencil.Forms.Views.Standard;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Stencil.Forms.Commanding.Commands
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

        public override Task<bool> ExecuteAsync(ICommandScope commandScope, object commandParameter, IDataViewModel dataViewModel)
        {
            return base.ExecuteFunctionAsync(nameof(ExecuteAsync), async delegate ()
            {
                NavigationData navigationData = this.ParseNavigationData<NavigationData>(commandParameter);
                Debug.WriteLine("---> Navigation to: " + navigationData.screen_name + " with target: " + commandScope.TargetMenuEntry);

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
