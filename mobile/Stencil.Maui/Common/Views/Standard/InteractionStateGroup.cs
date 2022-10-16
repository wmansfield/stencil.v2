using System;
using System.Collections.Generic;
using System.Text;

namespace Stencil.Maui.Views.Standard
{
    public class InteractionStateGroup
    {
        public InteractionStateGroup()
        {
            this.state_maps = new List<InteractionStateMap>();
        }
        public List<InteractionStateMap> state_maps { get; set; }
        public string interaction_group { get; set; }
        public string state_key { get; set; }
        public string value_key { get; set; }
    }
}
