using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Forms.Data
{
    public interface IStencilDatabaseConnector
    {
        IStencilDatabase OpenStencilDatabase();
    }
}
