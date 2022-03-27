using Stencil.Forms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Commanding.Commands
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
                bool reload = false;
                if (commandParameter != null)
                {
                    bool.TryParse(commandParameter.ToString(), out reload);
                }

                await this.API.Router.PopViewAsync(reload);
                return true;
            });
        }


    }
}
