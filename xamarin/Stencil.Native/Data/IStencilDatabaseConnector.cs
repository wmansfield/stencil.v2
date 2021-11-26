using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Data
{
    public interface IStencilDatabaseConnector
    {
        IStencilDatabase OpenStencilDatabase();
    }
}
