using Stencil.Maui.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Commands
{
    public abstract class StarterAppCommand : BaseAppCommand<StarterAPI>
    {
        public StarterAppCommand(string trackPrefix)
            : base(StarterAPI.Instance, trackPrefix)
        {

        }
    }
}
