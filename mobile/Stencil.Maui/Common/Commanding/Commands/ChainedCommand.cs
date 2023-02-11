using Newtonsoft.Json;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stencil.Maui.Commanding.Commands
{
    public class ChainedCommand : BaseAppCommand<StencilAPI>
    {
        public ChainedCommand()
            : base(StencilAPI.Instance, nameof(ChainedCommand))
        {

        }

        public override bool AlertErrors
        {
            get
            {
                return true;
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
                if(commandParameter != null)
                {
                    ChainInfo chainInfo = commandParameter as ChainInfo;
                    if(chainInfo == null)
                    {
                        string stringParameter = commandParameter.ToString();
                        if(stringParameter.StartsWith("{"))
                        {
                            chainInfo = JsonConvert.DeserializeObject<ChainInfo>(stringParameter);
                        }
                    }

                    if (chainInfo != null)
                    {
                        bool success = await commandScope.CommandProcessor.ExecuteCommandAsync(commandScope, chainInfo.command_name, chainInfo.command_parameter, dataViewModel, token);
                        if(success)
                        {
                            if(!string.IsNullOrWhiteSpace(chainInfo.success_command_name))
                            {
                                await commandScope.CommandProcessor.ExecuteCommandAsync(commandScope, chainInfo.success_command_name, chainInfo.success_command_parameter, dataViewModel, token);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(chainInfo.fail_command_name))
                            {
                                await commandScope.CommandProcessor.ExecuteCommandAsync(commandScope, chainInfo.fail_command_name, chainInfo.fail_command_parameter, dataViewModel, token);
                            }
                        }
                        return success;
                    }
                }
                return false;
            });
        }

        public class ChainInfo
        {
            public string command_name { get; set; }
            public string command_parameter { get; set; }

            public string success_command_name { get; set; }
            public string success_command_parameter { get; set; }

            public string fail_command_name { get; set; }
            public string fail_command_parameter { get; set; }
        }

    }
}
