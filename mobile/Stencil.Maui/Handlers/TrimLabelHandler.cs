using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stencil.Maui.Controls;

namespace Stencil.Maui.Handlers
{
    public partial class TrimLabelHandler : LabelHandler
    {
        public static IPropertyMapper<TrimLabel, TrimLabelHandler> PropertyMapper = new PropertyMapper<TrimLabel, TrimLabelHandler>(LabelHandler.ViewMapper);
        
        public TrimLabelHandler() : base(PropertyMapper)
        {

        }

    }
}
