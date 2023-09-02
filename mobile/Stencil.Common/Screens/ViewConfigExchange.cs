using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stencil.Common.Screens
{
    public class ViewConfigExchange : IViewConfig
    {
        public string library { get; set; }
        public string component { get; set; }
        public string tag { get; set; }
        public string configuration_json { get; set; }
        public SectionConfigExchange[] sections { get; set; }
        public ViewConfigExchange[] encapsulated_views { get; set; }
        ISectionConfig[] IViewConfig.sections 
        { 
            get
            {
                if (this.sections == null)
                {
                    return null;
                }
                return this.sections.AsEnumerable<ISectionConfig>().ToArray();
            }
            set
            {
                if (value != null)
                {
                    this.sections = value.Cast<SectionConfigExchange>().ToArray();
                }
            }
        }
        IViewConfig[] IViewConfig.encapsulated_views 
        {
            get
            {
                if (this.encapsulated_views == null)
                {
                    return null;
                }
                return this.encapsulated_views.AsEnumerable<IViewConfig>().ToArray();
            }
            set
            {
                if (value != null)
                {
                    this.encapsulated_views = value.Cast<ViewConfigExchange>().ToArray();
                }
            }
        }
    }
}
