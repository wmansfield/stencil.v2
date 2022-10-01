using Newtonsoft.Json;
using Stencil.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Stencil.Forms.Commanding.Commands
{
    public class CopyToClipboardCommand : BaseAppCommand<StencilAPI>
    {
        public CopyToClipboardCommand()
            : base(StencilAPI.Instance, "CopyToClipboardCommand")
        {

        }

        public override bool AlertErrors
        {
            get
            {
                return false;
            }
        }

        public override Task<string> CanExecuteAsync(ICommandScope commandScope, IDataViewModel dataViewModel)
        {
            return base.ExecuteFunction(nameof(CanExecuteAsync), delegate ()
            {
                return Task.FromResult((string)null);
            });
        }

        public override Task<bool> ExecuteAsync(ICommandScope commandScope, object commandParameter, IDataViewModel dataViewModel)
        {
            return base.ExecuteFunctionAsync(nameof(ExecuteAsync), async delegate ()
            {
                if (commandParameter != null)
                {
                    try
                    {
                        await Clipboard.SetTextAsync(commandParameter.ToString());

                        string found = await Clipboard.GetTextAsync();
                        if (found == commandParameter.ToString())
                        {
                            this.API.Alerts.Toast(this.API.Localize(StencilLanguageTokens.Copied_To_Clipboard.ToString(), "Copied to clipboard."), TimeSpan.FromSeconds(3));
                            return true;
                        }
                    }
                    catch { }
                }
                return false;
            });
        }
    }
}

