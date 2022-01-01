using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Native.Commanding.Commands
{
    public class NavigatePopCommand : BaseNavigationCommand<StencilAPI>
    {
        public NavigatePopCommand()
            : base(StencilAPI.Instance, nameof(NavigatePopCommand))
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
            return base.ExecuteFunctionAsync(nameof(ExecuteAsync), async delegate ()
            {
                await this.API.Router.PopViewAsync();
                return true;
            });
        }


    }
}
