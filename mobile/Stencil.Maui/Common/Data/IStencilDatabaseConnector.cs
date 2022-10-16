using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Data
{
    public interface IStencilDatabaseConnector
    {
        IStencilDatabase OpenStencilDatabase();
    }
}
