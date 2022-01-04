using Newtonsoft.Json;
using Stencil.Native.Commanding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Views.Standard
{
    public abstract class PreparedBingingContext : TrackedClass, IDataViewItemReference
    {
        public PreparedBingingContext(string trackPrefix)
            : base(trackPrefix)
        {

        }

        [JsonIgnore]
        public IDataViewItem DataViewItem { get; set; }


        [JsonIgnore]
        public ICommandScope CommandScope { get; set; }
    }
}
