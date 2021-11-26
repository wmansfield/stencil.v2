using Newtonsoft.Json;

namespace Stencil.Native.Screens
{
    public class ViewConfigBuilder<TConfiguration> : ViewConfig
        where TConfiguration : class
    {
        private TConfiguration _configuration;
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
