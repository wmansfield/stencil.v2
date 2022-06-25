using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Forms.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Commanding.Commands
{
    public abstract class BaseNavigationCommand<TAPI> : BaseAppCommand<TAPI>
        where TAPI : StencilAPI
    {
        public BaseNavigationCommand(TAPI api, string trackPrefix)
            : base(api, trackPrefix)
        {

        }
        protected virtual TNavigationData ParseNavigationData<TNavigationData>(object commandParameter)
            where TNavigationData : class, INavigationData, new()
        {
            return base.ExecuteFunction(nameof(ParseNavigationData), delegate ()
            {
                // this paradigm is used elsewhere, use caution changing
                if (commandParameter != null)
                {
                    TNavigationData existing = commandParameter as TNavigationData;
                    if (existing != null)
                    {
                        return existing;
                    }

                    string stringContents = commandParameter.ToString();
                    if (stringContents.StartsWith("{"))
                    {
                        return JsonConvert.DeserializeObject<TNavigationData>(stringContents);
                    }
                    else
                    {
                        return new TNavigationData()
                        {
                            screen_name = stringContents
                        };
                    }
                }
                return null;
            });
        }
    }
}
