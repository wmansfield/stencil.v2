using Stencil.Maui.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Data
{
    public class StarterDatabaseConnector : IStarterDatabaseConnector
    {
        public IStarterDatabase OpenStarterDatabase()
        {
            return new StarterDatabase(StarterApplication.Instance.GenerateRealm());
        }

        IStencilDatabase IStencilDatabaseConnector.OpenStencilDatabase()
        {
            return this.OpenStarterDatabase();
        }
    }
}
