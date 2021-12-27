using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Native.Commanding
{
    //TODO:MUST: AppCommand naming?
    public interface IAppCommand : IDynamicCommand<bool>
    {
    }
}
