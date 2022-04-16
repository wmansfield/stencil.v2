using Newtonsoft.Json;
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
                PopOptions options = commandParameter as PopOptions;
                if(options == null)
                {
                    options = new PopOptions();
                }
                if (commandParameter != null)
                {
                    string commandParameterString = commandParameter.ToString();
                    if(commandParameterString.StartsWith("{"))
                    {
                        options = JsonConvert.DeserializeObject<PopOptions>(commandParameterString);
                    }
                    else if(commandParameterString.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        options.reload = true;
                    }
                    else if (commandParameterString.Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        options.reload = false;
                    }
                    else
                    {
                        if (int.TryParse(commandParameterString, out int iterations))
                        {
                            if(iterations.ToString() == commandParameterString)
                            {
                                options.iterations = iterations;
                            }
                        }
                    }
                }

                if(options.iterations < 1)
                {
                    options.iterations = 1;
                }

                for (int i = 0; i < options.iterations; i++) 
                {
                    bool reload = false;
                    if(i == options.iterations - 1)
                    {
                        reload = options.reload; // only reload last one
                    }
                    await this.API.Router.PopViewAsync(reload);
                }
                return true;
            });
        }


        public class PopOptions
        {
            public bool reload { get; set; }
            public int iterations { get; set; }
        }

    }
}
