using Newtonsoft.Json;
using Stencil.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Commanding.Commands
{
    public class ChangeStateCommand : BaseAppCommand<StencilAPI>
    {
        public ChangeStateCommand()
            : base(StencilAPI.Instance, nameof(ChangeStateCommand))
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
            return base.ExecuteFunction(nameof(ExecuteAsync), delegate ()
            {
                if(commandParameter != null && dataViewModel != null)
                {
                    StateInfo stateInfo = commandParameter as StateInfo;
                    if(stateInfo == null)
                    {
                        string stringParameter = commandParameter.ToString();
                        if(stringParameter.StartsWith("{"))
                        {
                            stateInfo = JsonConvert.DeserializeObject<StateInfo>(stringParameter);
                        }
                        else
                        {
                            string[] values = stringParameter.Split(',').Select(x => x.Trim()).ToArray();
                            if(values.Length == 3)
                            {
                                stateInfo = new StateInfo()
                                {
                                    group = values[0],
                                    name = values[1],
                                    state = values[2]
                                };
                            }
                        }
                    }

                    if (stateInfo != null)
                    {
                        List<IStateEmitter> emitters = dataViewModel.StateEmitters.Where(x => x.InteractionGroup == stateInfo.group).ToList();
                        if (emitters.Count > 0)
                        {
                            // specific emitter, presume its a secondary invocation mechanism
                            foreach (IStateEmitter emitter in emitters)
                            {
                                emitter.ChangeStateAsync(stateInfo.name, stateInfo.state);
                            }
                        }
                        else
                        {
                            // no specific emitter, just raise it
                            dataViewModel?.RaiseStateChange(stateInfo.group, stateInfo.name, stateInfo.state);
                        }
                        return Task.FromResult(true);
                    }
                }
                return Task.FromResult(false);
            });
        }

        public class StateInfo
        {
            public string group { get; set; }
            public string name { get; set; }
            public string state { get; set; }
        }


    }
}
