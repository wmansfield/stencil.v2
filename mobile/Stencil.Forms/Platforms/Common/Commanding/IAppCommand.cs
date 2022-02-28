using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Forms.Commanding
{
    //TODO:MUST: AppCommand naming?
    public interface IAppCommand : IDynamicCommand<bool>
    {
    }
}
