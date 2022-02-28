using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Placeholder.SDK
{
    public static class SDKUtility
    {
        public static bool IsValidUrl(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            Uri parsed = null;
            if (!text.ToLower().StartsWith("http"))
            {
                text = "http://" + text;
            }
            // toLower for ease of validation, we dont use the result anyway
            return Uri.TryCreate(text.ToLower(), UriKind.Absolute, out parsed) && (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps);
        }
        public static bool IsValidNumber(string text)
        {
            decimal value = 0;
            return decimal.TryParse(text, System.Globalization.NumberStyles.Any, null, out value);
        }
        public static bool IsValidInteger(string text)
        {
            decimal decimalValue = 0;
            if (decimal.TryParse(text, System.Globalization.NumberStyles.Any, null, out decimalValue))
            {
                int intValue = 0;
                if (int.TryParse(text, System.Globalization.NumberStyles.Any, null, out intValue))
                {
                    return intValue == (int)decimalValue;
                }
            }
            return false;
        }

        public static bool IsValidEmail(string email, out string parsedEmail)
        {
            parsedEmail = email;
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            int ix = email.LastIndexOf("@");
            if (ix < 0)
            {
                return false;
            }
            try
            {
                // translate global
                string name = email.Substring(0, ix);
                string domain = email.Substring(ix + 1);
                domain = new IdnMapping().GetAscii(domain);
                parsedEmail = string.Format("{0}@{1}", name, domain);

                // verify valid
                return Regex.IsMatch(parsedEmail,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(10000));
            }
            catch
            {
                return false;// gulp
            }
        }
    }
}
