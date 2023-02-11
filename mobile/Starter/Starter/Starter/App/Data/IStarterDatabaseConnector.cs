using Stencil.Maui.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Data
{
    public interface IStarterDatabaseConnector : IStencilDatabaseConnector
    {
        IStarterDatabase OpenStarterDatabase();
    }
}
