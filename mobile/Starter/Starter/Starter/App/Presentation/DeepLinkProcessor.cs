using Newtonsoft.Json;
using Starter.App.Commands;
using Starter.App.Screens;
using Stencil.Common.Screens;
using Stencil.Maui;
using Stencil.Maui.Commanding;
using Stencil.Maui.Commanding.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App.Presentation
{
    public class DeepLinkProcessor
    {
        /// <summary>
        /// Returns true if success
        /// </summary>
        public static bool NavigateIfPossible(string linkText)
        {
            return CoreUtility.ExecuteFunction("NavigateIfPossible", delegate ()
            {
                if (!linkText.StartsWith(StarterAssumptions.URL_SCHEME, StringComparison.OrdinalIgnoreCase) && !linkText.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    // silly, inject the domain name with anything so below code works.
                    int ix = linkText.IndexOf("://");
                    if (ix > -1)
                    {
                        linkText = linkText.Substring(0, ix + 3) + "stencilstart.com/" + linkText.Substring(ix + 3);
                    }
                }
                Uri uri = new Uri(linkText.ToLower());

                if (linkText.Contains("/launch")) // launch only
                {
                    return true;
                }
                if (linkText.Contains("/v/"))
                {
                    //stencilstart://v/a/{param1}/{param2?}
                    if (uri.Segments[1] == "s/") // Sample
                    {
                        return ProcessSampleScreen(uri);
                    }
                    return true;
                }
                return false;
            });
        }

        private static bool ProcessSampleScreen(Uri uri)
        {
            return CoreUtility.ExecuteFunction(nameof(ProcessSampleScreen), delegate ()
            {
                if (StarterAPI.Instance.Application.CurrentAccount != null)
                {
                    //stencilstart://v/s/{param1}/{param2?}
                    string param1 = null;
                    string param2 = null;
                    if (uri.Segments.Length > 2)
                    {
                        param1 = uri.Segments[2].Trim('/');
                    }
                    if (uri.Segments.Length > 3)
                    {
                        param2 = uri.Segments[3].Trim('/');
                    }
                    if (!string.IsNullOrEmpty(param1) && !string.IsNullOrEmpty(param2))
                    {
                        CommandScope commandScope = new CommandScope(StarterAPI.Instance.CommandProcessor);

                        string commandName = WellKnownCommands.CHAINED_COMMAND;
                        string commandParameter = JsonConvert.SerializeObject(new ChainedCommand.ChainInfo()
                        {
                            command_name = WellKnownCommands.APP_NAVIGATE_ROOT,
                            success_command_name = WellKnownCommands.APP_NAVIGATE_PUSH,
                            success_command_parameter = JsonConvert.SerializeObject(new NavigationData()
                            {
                                screen_name = WellKnownScreens.SAMPLE,
                                screen_parameter = string.Format("{0}/{1}", param1, param2)
                            })
                        });

                        StarterAPI.Instance.CommandProcessor.ExecuteCommandAsync(commandScope, commandName, commandParameter, null);
                        return true;
                    }
                }

                return false;
            });
        }
    }
}
