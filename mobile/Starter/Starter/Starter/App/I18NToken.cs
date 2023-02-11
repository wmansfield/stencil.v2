using Stencil.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App
{
    public enum I18NToken
    {
        General_Done, //"Done"
        General_Day, //"day"
        General_Days, //"days"
        General_Hour, //"hour"
        General_Hours, //"hours"
        General_Mins, //"mins"
        General_Secs, //"secs"
        General_Seconds, //"seconds"
        General_Min, //"min"
        General_Sec, //"sec"
        General_Second, //"second"

        ScreenLoading_LoadingAccount, //"loading.."
    }

    public static class I18NTokenExtensions
    {
        public static string Localize(this StencilAPI api, I18NToken token, string defaultText, params object[] arguments)
        {
            return api?.Localize(token.ToString(), defaultText, arguments);
        }
    }
}
