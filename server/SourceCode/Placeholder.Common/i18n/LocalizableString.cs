using System;

namespace Placeholder.Common
{
    public class LocalizableString
    {
        public const string SERVER = "server";

        public static LocalizableString General_InvalidToken()
        {
            return new LocalizableString(SERVER, "general.InvalidToken", "Invalid or expired token provided.");
        }
        public static LocalizableString General_ValidationError()
        {
            return new LocalizableString(SERVER, "general.ValidationFailed", "Validation Failed. Please check entity.");
        }
        public static LocalizableString General_NotYetSupported()
        {
            return new LocalizableString(SERVER, "general.NotYetSupported", "Validation Failed. The type of item you have requested is not yet supported.");
        }
        public static LocalizableString General_ErrorProcessingRequest()
        {
            return new LocalizableString(SERVER, "general.ErrorProcessingRequest", "Error while processing request. Please check for success before trying again.");
        }
        public static LocalizableString General_MissingForSave()
        {
            return new LocalizableString(SERVER, "general.SaveMissing", "Error saving data. Please refresh and try again.");
        }
        public static LocalizableString General_AccessDenied_Create()
        {
            return new LocalizableString(SERVER, "general.CRUD_AccessDenied_Create", "Access Denied. Cannot create requested item. Please check security or contact an administrator.");
        }
        public static LocalizableString General_AccessDenied_Retrieve()
        {
            return new LocalizableString(SERVER, "general.CRUD_AccessDenied_Retrieve", "Access Denied. Cannot retrieve requested item. Please check security or contact an administrator.");
        }
        public static LocalizableString General_AccessDenied_List()
        {
            return new LocalizableString(SERVER, "general.CRUD_AccessDenied_List", "Access Denied. Cannot list requested items. Please check security or contact an administrator.");
        }
        public static LocalizableString General_AccessDenied_Search()
        {
            return new LocalizableString(SERVER, "general.CRUD_AccessDenied_Search", "Access Denied. Cannot search requested items. Please check security or contact an administrator.");
        }
        public static LocalizableString General_AccessDenied_Update()
        {
            return new LocalizableString(SERVER, "general.CRUD_AccessDenied_Update", "Access Denied. Cannot updated requested item. Please check security or contact an administrator.");
        }
        public static LocalizableString General_AccessDenied_Delete()
        {
            return new LocalizableString(SERVER, "general.CRUD_AccessDenied_Delete", "Access Denied. Cannot delete requested item. Please check security or contact an administrator.");
        }


        public LocalizableString(string area, string token, string defaultText, params object[] arguments)
        {
            this.Area = area;
            this.Token = token;
            this.DefaultText = defaultText;
            this.Arguments = arguments;
        }
        public LocalizableString(string area, string token, string defaultText)
        {
            this.Area = area;
            this.Token = token;
            this.DefaultText = defaultText;
        }
        public string Area { get; set; }
        public string Token { get; set; }
        public string DefaultText { get; set; }

        public object[] Arguments { get; set; }
    }
}
