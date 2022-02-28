using System;
using System.Collections.Generic;
using Placeholder.Common;

namespace Placeholder.Common.Exceptions
{
    [System.Serializable]
    public class UIException : System.Exception
    {
        public UIException(LocalizableString message)
            : base(message.DefaultText)
        {
            this.LocalizableMessage = message;
        }
        public UIException(LocalizableString message, Dictionary<string, LocalizableString> errors)
            : base(message.DefaultText)
        {
            this.LocalizableMessage = message;
            this.Errors = errors;
        }
        public UIException(LocalizableString message, Dictionary<string, Dictionary<int, LocalizableString>> orderedErrors)
            : base(message.DefaultText)
        {
            this.LocalizableMessage = message;
            this.OrderedErrors = orderedErrors;
        }
        public UIException(LocalizableString message, Dictionary<string, Dictionary<string, LocalizableString>> nestedErrors)
            : base(message.DefaultText)
        {
            this.LocalizableMessage = message;
            this.NestedErrors = nestedErrors;
        }

        public UIException(LocalizableString message, Exception inner)
            : base(message.DefaultText, inner)
        {
            this.LocalizableMessage = message;
        }
        protected UIException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }


        public int? ErrorCode { get; private set; }
        public LocalizableString LocalizableMessage { get; private set; }
        public Dictionary<string, LocalizableString> Errors { get; set; }
        public Dictionary<string, Dictionary<string, LocalizableString>> NestedErrors { get; set; }
        public Dictionary<string, Dictionary<int, LocalizableString>> OrderedErrors { get; set; }

    }
}
