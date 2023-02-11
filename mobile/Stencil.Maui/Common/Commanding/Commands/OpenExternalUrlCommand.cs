using Microsoft.Maui.ApplicationModel;
using Newtonsoft.Json;
using Stencil.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stencil.Maui.Commanding.Commands
{
    public class OpenExternalUrlCommand : BaseAppCommand<StencilAPI>
    {
        public OpenExternalUrlCommand()
            : base(StencilAPI.Instance, nameof(OpenExternalUrlCommand))
        {

        }

        public override bool AlertErrors
        {
            get
            {
                return true;
            }
        }

        public override Task<string> CanExecuteAsync(ICommandScope commandScope, IDataViewModel dataViewModel)
        {
            return base.ExecuteFunction(nameof(CanExecuteAsync), delegate ()
            {
                return Task.FromResult((string)null);
            });
        }

        public override Task<bool> ExecuteAsync(ICommandScope commandScope, object commandParameter, IDataViewModel dataViewModel, CancellationToken token = default)
        {
            return base.ExecuteFunctionAsync(nameof(ExecuteAsync), async delegate ()
            {
                if (commandParameter != null && dataViewModel != null)
                {
                    Uri target = null;
                    try
                    {
                        // encoded url safety
                        string targetUrl = commandParameter.ToString();
                        if (targetUrl.StartsWith("https%3a%2f%2f") || targetUrl.StartsWith("http%3a%2f%2f"))
                        {
                            targetUrl = WebUtility.UrlDecode(targetUrl);
                        }
                        target = new Uri(targetUrl);

                    }
                    catch
                    {
                        // gulp
                        return false;
                    }
                    try
                    {
                        await Browser.OpenAsync(target, BrowserLaunchMode.External);
                        return true;
                    }
                    catch
                    {
                        // gulp
                        return false;
                    }
                }
                return false;
            });
        }
    }
}
