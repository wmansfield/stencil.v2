using Newtonsoft.Json;
using Placeholder.Primary.UI;
using Placeholder.SDK;
using Placeholder.SDK.Models;
using Placeholder.SDK.Models.Requests;
using Stencil.Common;
using Stencil.Common.Commands;
using Stencil.Common.Screens;
using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.Unity;
using Placeholder.Primary;

namespace Placeholder.Primary.UI
{
    public class UIComposer : ChokeableClass
    {
        public UIComposer(IFoundation foundation)
            : base(foundation)
        {
            this.API = new PlaceholderAPI(foundation);
            this.Cache15 = new AspectCache("UIComposer.Cache", foundation, new ExpireStaticLifetimeManager("UIComposer.Cache15", TimeSpan.FromMinutes(15)));
        }

        public AspectCache Cache15 { get; set; }
        public PlaceholderAPI API { get; set; }



        public ScreenConfigExchange GenerateDebugScreenConfig(NavigationData navigationData)
        {
            return base.ExecuteFunction(nameof(GenerateDebugScreenConfig), delegate ()
            {
                ScreenConfigExchange result = new ScreenConfigExchange()
                {
                    SuppressPersist = false,
                    ScreenName = "debug",
                    ScreenParameter = string.Empty,
                    ScreenNavigationData = navigationData,
                    Lifetime = Lifetime.until_expired,
                    ExpireUTC = DateTime.UtcNow.AddMinutes(15),
                    VisualConfig = new VisualConfig()
                    {
                        BackgroundColor = "#ffffff",
                    },
                    IsMenuSupported = true,
                    MenuConfigs = null,
                };

                result.HeaderConfigs.Add(new ViewConfigBuilder<WellKnownComponents.H2.Config>()
                {
                    component = WellKnownComponents.H2.NAME,
                    configuration = new WellKnownComponents.H2.Config()
                    {
                        TextColor = "#FFFF00",
                        Text = "Header"
                    }
                });

                return result;
            });
        }
        
    }
}
