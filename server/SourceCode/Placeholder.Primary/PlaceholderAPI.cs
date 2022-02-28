using System;
using Zero.Foundation;

namespace Placeholder.Primary
{
    public class PlaceholderAPI : BaseClass
    {
        public PlaceholderAPI(IFoundation foundation)
            : base(foundation)
        {
            this.Direct = new PlaceholderAPIDirect(foundation);
            this.Index = new PlaceholderAPIIndex(foundation);
            this.Integration = new PlaceholderAPIIntegration(foundation);
            this.Store = new PlaceholderAPIStore(foundation);
        }
        public PlaceholderAPIDirect Direct { get; set; }
        public PlaceholderAPIIndex Index { get; set; }
        public PlaceholderAPIIntegration Integration { get; set; }
        public PlaceholderAPIStore Store { get; set; }


    }
}
