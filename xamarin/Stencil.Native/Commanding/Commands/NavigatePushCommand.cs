﻿using Stencil.Native.Views;
using Stencil.Native.Views.Standard;
using System.Threading.Tasks;

namespace Stencil.Native.Commanding.Commands
{
    public class NavigatePushCommand : BaseAppCommand<StencilAPI>
    {
        public NavigatePushCommand()
            : base(StencilAPI.Instance, nameof(NavigatePushCommand))
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
            return base.ExecuteFunctionAsync(nameof(ExecuteAsync), async delegate ()
            {
                IDataViewModel dataViewModel = await this.API.Screens.GenerateScreenAsync(this.API.CommandProcessor, $"{commandParameter}");
                StandardDataView dataView = new StandardDataView(dataViewModel);
                await this.API.Router.PushViewAsync(dataView, commandScope.TargetMenuEntry);
                return true;
            });
        }
    }
}