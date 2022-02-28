using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Forms.Effects
{
    public class BouncelessListViewEffect : RoutingEffect
    {
        public BouncelessListViewEffect()
            : base(Effects.GROUP + "." + NAME)
        {
        }

        public const string NAME = "BouncelessListView";

    }
}
