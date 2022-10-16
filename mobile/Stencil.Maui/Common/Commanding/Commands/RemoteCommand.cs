using Newtonsoft.Json;
using Stencil.Common;
using Stencil.Common.Commands;
using Stencil.Maui.Commanding;
using Stencil.Maui.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Maui.Platforms.Common.Commanding.Commands
{
    public abstract class RemoteCommand<TAPI, TInput> : BaseAppCommand<TAPI>
        where TAPI : StencilAPI
        where TInput : RemoteCommandInput, new()
    {
        public RemoteCommand(TAPI api, string trackPrefix) 
            : base(api, trackPrefix)
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
            return base.ExecuteFunctionAsync(nameof(CanExecuteAsync), async delegate ()
            {
                string inputError = await base.ValidateUserInputValuesAsync(commandScope, RemoteCommandInput.INPUT_GROUP);
                if (!string.IsNullOrWhiteSpace(inputError))
                {
                    return inputError;
                }
                return string.Empty;
            });
        }

        public override Task<bool> ExecuteAsync(ICommandScope commandScope, object commandParameter, IDataViewModel dataViewModel)
        {
            return base.ExecuteFunctionAsync(nameof(ExecuteAsync), async delegate ()
            {
                TInput input = this.PrepareInput(commandParameter);
                input = this.FillUserValues(commandScope, dataViewModel, input);

                RemoteCommandResponse result = await this.ExecuteCommandRemotelyAsync(commandScope, input, dataViewModel);
                if(result == null)
                {
                    return false;
                }

                if(result.success)
                {
                    if(!string.IsNullOrWhiteSpace(result.command_name))
                    {
                        try
                        {
                            await this.API.CommandProcessor.ExecuteCommandAsync(commandScope, result.command_name, result.command_parameter, dataViewModel);
                        }
                        catch (Exception ex)
                        {
                            if (result.command_show_errors)
                            {
                                await this.API.Alerts.AlertAsync(ex.Message, "Error", "OK"); //TODO:Localize?
                            }
                        }
                    }
                }
                else
                {
                    if(this.AlertErrors)
                    {
                        await this.API.Alerts.AlertAsync(result.message, "Error", "OK"); //TODO:Localize?
                    }
                }
                return result.success;
            });
        }

        public abstract Task<RemoteCommandResponse> ExecuteCommandRemotelyAsync(ICommandScope commandScope, TInput input, IDataViewModel dataViewModel);
        
        protected virtual TInput PrepareInput(object commandParameter)
        {
            return base.ExecuteFunction(nameof(PrepareInput), delegate ()
            {
                TInput configuration = null;
                if (commandParameter != null)
                {
                    string stringParameter = commandParameter.ToString();
                    if(stringParameter.StartsWith("{"))
                    {
                        configuration = JsonConvert.DeserializeObject<TInput>(commandParameter.ToString());
                    }
                    else
                    {
                        configuration = new TInput()
                        {
                            command_name = stringParameter
                        };
                    }
                }
                if (configuration == null)
                {
                    configuration = new TInput();
                }
                
                return configuration;
            });
        }
        protected virtual TInput FillUserValues(ICommandScope scope, IDataViewModel dataViewModel, TInput input)
        {
            return base.ExecuteFunction(nameof(FillUserValues), delegate ()
            {
                if (input == null)
                {
                    input = new TInput();
                }
                ConcurrentDictionary<string, ICommandField> values = this.ExtractCommandGroup(scope, RemoteCommandInput.INPUT_GROUP);
                foreach (KeyValuePair<string, ICommandField> item in values)
                {
                    input.user_values.Add(new InputPair()
                    {
                        name = item.Value.FieldName,
                        value = item.Value.GetFieldValue(),
                    });
                }

                return input;
            });
        }

    }
}
