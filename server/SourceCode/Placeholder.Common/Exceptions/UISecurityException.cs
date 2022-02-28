using System;
using System.Collections.Generic;

namespace Placeholder.Common.Exceptions
{
    [Serializable]
    public class UISecurityException : UIException
    {
        public UISecurityException(LocalizableString message) : base(message) { }
        public UISecurityException(LocalizableString message, Dictionary<string, LocalizableString> errors) : base(message, errors) { }
        public UISecurityException(LocalizableString message, Exception inner) : base(message, inner) { }

        protected UISecurityException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
