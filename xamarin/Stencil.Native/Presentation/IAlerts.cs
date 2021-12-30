using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stencil.Native.Presentation
{
    public interface IAlerts
    {
        Task AlertAsync(string message, string title = null, string okText = null, CancellationToken? cancelToken = null);
        IDisposable Toast(string title, TimeSpan? dismissTimer = null);

    }
}
