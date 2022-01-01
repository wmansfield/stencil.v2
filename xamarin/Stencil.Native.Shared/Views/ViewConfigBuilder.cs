using Newtonsoft.Json;
using Stencil.Native.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Native.Views
{
    public class ViewConfigBuilder<TConfiguration> : ViewConfig
        where TConfiguration : class
    {
        private TConfiguration _configuration;
        [JsonIgnore]
        public TConfiguration configuration
        {
            get
            {
                return _configuration;
            }
            set
            {
                _configuration = value;
                if (_configuration == null)
                {
                    this.configuration_json = null;
                }
                else
                {
                    this.configuration_json = JsonConvert.SerializeObject(value);
                }
            }
        }

    }
}
