using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Native.Data.Sync
{
    public interface IDataSync
    {
        bool Enabled { get; set; }

        Task OnAppStartAsync();
        Task OnAppResumeAsync();
        Task OnAppSleepAsync();
        Task OnSessionStartAsync();
        Task OnSessionEndAsync();
    }
}
