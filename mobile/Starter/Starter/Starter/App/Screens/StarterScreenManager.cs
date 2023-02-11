using Newtonsoft.Json;
using Stencil.Common;
using Stencil.Common.Markdown;
using Stencil.Common.Screens;
using Stencil.Common.Views;
using Stencil.Maui.Commanding;
using Stencil.Maui.Commanding.Commands;
using Stencil.Maui.Data;
using Stencil.Maui.Resourcing;
using Stencil.Maui.Screens;
using Stencil.Maui.Views;
using Stencil.Maui.Views.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starter.App.Commands;
using Starter.App.Data;
using Starter.App.Models;
using Starter.App.Views;

using st1 = Stencil.Maui.Views.Standard.v1_0;
using uv1 = Starter.App.Views.V1;
using sdk = Starter.SDK.Models;
using System.Net.Http;
using Starter.App.Cloud;

namespace Starter.App.Screens
{
    public partial class StarterScreenManager : ScreenManager<StarterAPI>
    {
        public StarterScreenManager()
            : base(StarterAPI.Instance)
        {

        }

        protected override DataTemplateSelector CreateDataTemplateSelector(ICommandScope scope)
        {
            return base.ExecuteFunction(nameof(CreateDataTemplateSelector), delegate ()
            {
                return new StarterComponentLibraryTemplateSelector(scope);
            });
        }

        protected override Task<ScreenConfig> GenerateMissingScreenConfigAsync(INavigationData navigationData)
        {
            return base.ExecuteFunctionAsync(nameof(GenerateMissingScreenConfigAsync), async delegate ()
            {
                ScreenConfig screenConfig = null;
                if (navigationData.screen_name == null)
                {
                    navigationData.screen_name = string.Empty;
                }

                switch (navigationData.screen_name)
                {
                    case WellKnownScreens.ONBOARDING:
                        screenConfig = this.GenerateOnboardingConfig();
                        break;
                    case WellKnownScreens.SAMPLE:
                        screenConfig = this.GenerateSampleConfig();
                        break;
                    default:
                        MockEnvelope<ScreenConfigExchange> result = await this.API.Cloud.RemoteScreenGetAsync(new sdk.MockInput()
                        {
                            navigation_data = new NavigationData()
                            {
                                screen_name = navigationData.screen_name,
                                screen_parameter = navigationData.screen_parameter,
                                last_retrieved_utc = navigationData.last_retrieved_utc,
                                data = navigationData.data
                            }
                        });

                        if (!result.success)
                        {
                            //TODO:MUST: Screen unavailable, show network teaser screen?
                        }
                        else
                        {
                            screenConfig = result.item.ToScreenConfig();
#if DEBUG
                            if (screenConfig != null)
                            {
                                //screenConfig.SuppressPersist = true;
                            }
#endif
                        }
                        break;
                }

                return screenConfig;

            });
        }

        protected override Task<IScreenConfig> PostProcessScreenConfigAsync(IScreenConfig screenConfig)
        {
            return base.ExecuteFunction(nameof(PostProcessScreenConfigAsync), delegate ()
            {
                IScreenConfig result = screenConfig;
                return Task.FromResult(result);
            });
        }


        private ScreenConfig GenerateOnboardingConfig()
        {
            return new ScreenConfig()
            {
                SuppressPersist = true,
                VisualConfig = new VisualConfig()
                {
                    Padding = new ThicknessInfo(0),
                    BackgroundColor = Colors.Transparent.ToArgbHex(),
                },
                IsMenuSupported = false,
                FooterConfigs = new List<IViewConfig>()
                {
                    new ViewConfigBuilder<st1.TriColumnViewContext>()
                    {
                        component = st1.TriColumnView.COMPONENT_NAME,
                        configuration = new st1.TriColumnViewContext()
                        {
                            HeightRequest = 100,
                            Column1Config = new st1.ColumnConfig()
                            {
                                HorizontalOptions = LayoutOptions.End,
                            },
                            Column2Config = new st1.ColumnConfig()
                            {
                                HorizontalOptions = LayoutOptions.Center,
                            },
                            Column3Config = new st1.ColumnConfig()
                            {
                                HorizontalOptions = LayoutOptions.Start,
                            }
                        },
                        encapsulated_views = new IViewConfig[]
                        {
                            new ViewConfigBuilder<st1.SimpleButtonContext>()
                            {
                                component = st1.SimpleButton.COMPONENT_NAME,
                                configuration = new st1.SimpleButtonContext()
                                {
                                    Text = "PREV", //this.API.Localize(..),
                                    TextColor = AppColors.PrimaryBlack,
                                    CommandName = WellKnownCommands.STATE_CHANGE,
                                    CommandParameter = $"onboard_carousel,move,-1",
                                    InteractionStateGroups = new List<InteractionStateGroup>()
                                    {
                                        new InteractionStateGroup()
                                        {
                                            interaction_group = "onboard_carousel",
                                            state_key = st1.Carousel.STATE_KEY_POSITION,
                                            value_key = st1.SimpleButtonContext.INTERACTION_KEY_HIDDEN,
                                            state_maps = new List<InteractionStateMap>()
                                            {
                                                new InteractionStateMap()
                                                {
                                                    state_operator = InteractionStateOperator.equals,
                                                    state = "0",
                                                    value_format = "true",
                                                },
                                                new InteractionStateMap()
                                                {
                                                    state_operator = InteractionStateOperator.not_equals,
                                                    state = "0",
                                                    value_format = "false"
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new ViewConfigBuilder<st1.IndicatorContext>()
                            {
                                component = st1.Indicator.COMPONENT_NAME,
                                configuration = new st1.IndicatorContext()
                                {
                                    Color = StarterColors.Current.TextColor,
                                    SelectedColor = StarterColors.Current.ButtonBackgroundFaded,
                                    ItemCount = 3,
                                    IndicatorSize = 20,
                                    Margin = new Thickness(0, 15),
                                    InteractionStateGroups = new List<InteractionStateGroup>()
                                    {
                                        new InteractionStateGroup()
                                        {
                                            interaction_group = "onboard_carousel",
                                            state_key = st1.Carousel.STATE_KEY_POSITION,
                                            value_key = st1.IndicatorContext.INTERACTION_KEY_SELECTED_POSITION,
                                            state_maps = new List<InteractionStateMap>()
                                            {
                                                new InteractionStateMap()
                                                {
                                                    state_operator = InteractionStateOperator.any,
                                                    value_format = "{0}",
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new ViewConfigBuilder<st1.SimpleButtonContext>()
                            {
                                component = st1.SimpleButton.COMPONENT_NAME,
                                configuration = new st1.SimpleButtonContext()
                                {
                                    Text = "NEXT",//this.API.Localize(..),
                                    TextColor = AppColors.PrimaryBlack,
                                    CommandName = WellKnownCommands.STATE_CHANGE,
                                    CommandParameter = $"onboard_carousel,move,1"
                                }
                            }
                        }
                    },
                    new ViewConfigBuilder<st1.SingleColumnViewContext>()
                    {
                        component = st1.SingleColumnView.COMPONENT_NAME,
                        configuration = new st1.SingleColumnViewContext()
                        {
                            HeightRequest = 50,
                            Column1Config = new st1.ColumnConfig()
                            {
                                HorizontalOptions = LayoutOptions.Center,
                            }
                        },
                        encapsulated_views = new IViewConfig[]
                        {
                            new ViewConfigBuilder<st1.SimpleButtonContext>()
                            {
                                component = st1.SimpleButton.COMPONENT_NAME,
                                configuration = new st1.SimpleButtonContext()
                                {
                                    Text = "Skip", //this.API.Localize(...),
                                    TextColor = AppColors.PrimaryBlack,
                                    CommandName = WellKnownCommands.APP_NAVIGATE_PUSH,
                                    CommandParameter = WellKnownScreens.SAMPLE,
                                }
                            }
                        }
                    }
                },
                ViewConfigs = new List<IViewConfig>()
                {
                    new ViewConfigBuilder<st1.CarouselContext>()
                    {
                        component = st1.Carousel.COMPONENT_NAME,
                        configuration = new st1.CarouselContext()
                        {
                            HeightRequest = 600,
                            BackgroundColor = "#00000000",
                            InteractionGroup = "onboard_carousel",
                            VerticalOptions = LayoutOptions.Fill,
                            OverMoveCommandName = WellKnownCommands.APP_NAVIGATE_PUSH,
                            OverMoveCommandParameter = WellKnownScreens.SAMPLE,
                            UnderMoveCommandName = WellKnownCommands.APP_NAVIGATE_PUSH,
                            UnderMoveCommandParameter = WellKnownScreens.SAMPLE,
                        },
                        sections = new SectionConfig[]
                        {
                            new SectionConfig()
                            {
                                ViewConfigs = new List<IViewConfig>()
                                {
                                    new ViewConfigBuilder<st1.SpacerContext>()
                                    {
                                        component = st1.Spacer.COMPONENT_NAME,
                                        configuration = new st1.SpacerContext()
                                        {
                                            Height = 80
                                        }
                                    },
                                    new ViewConfigBuilder<st1.H1Context>()
                                    {
                                        component = st1.H1.COMPONENT_NAME,
                                        configuration = new st1.H1Context()
                                        {
                                            Text = "Welcome", // this.API.Localize(..)
                                        }
                                    },
                                    new ViewConfigBuilder<st1.SpacerContext>()
                                    {
                                        component = st1.Spacer.COMPONENT_NAME,
                                        configuration = new st1.SpacerContext()
                                        {
                                            Height = 30
                                        }
                                    },
                                    new ViewConfigBuilder<st1.CenterTextContext>()
                                    {
                                        component = st1.CenterText.COMPONENT_NAME,
                                        configuration = new st1.CenterTextContext()
                                        {
                                            Text = "Welcome do data driven ui",//this.API.Localize(..)
                                            TextColor = StarterColors.Current.TextColor,
                                            FontSize = 15,
                                            Center = true,
                                            ContentWidth = "200"
                                        }
                                    },
                                }
                            },
                            new SectionConfig()
                            {
                                ViewConfigs = new List<IViewConfig>()
                                {
                                    new ViewConfigBuilder<st1.SpacerContext>()
                                    {
                                        component = st1.Spacer.COMPONENT_NAME,
                                        configuration = new st1.SpacerContext()
                                        {
                                            Height = 80
                                        }
                                    },
                                    new ViewConfigBuilder<st1.H1Context>()
                                    {
                                        component = st1.H1.COMPONENT_NAME,
                                        configuration = new st1.H1Context()
                                        {
                                            Text = "Feel free to..",//this.API.Localize(..)
                                            TextColor = StarterColors.Current.TextColor
                                        }
                                    }
                                }
                            },
                            new SectionConfig()
                            {
                                ViewConfigs = new List<IViewConfig>()
                                {
                                    new ViewConfigBuilder<st1.SpacerContext>()
                                    {
                                        component = st1.Spacer.COMPONENT_NAME,
                                        configuration = new st1.SpacerContext()
                                        {
                                            Height = 80
                                        }
                                    },
                                    new ViewConfigBuilder<st1.H1Context>()
                                    {
                                        component = st1.H1.COMPONENT_NAME,
                                        configuration = new st1.H1Context()
                                        {
                                            Text = "..remove this data",//this.API.Localize(..)
                                            TextColor = StarterColors.Current.TextColor
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

#if DEBUG
        private ScreenConfig GenerateSampleConfig()
        {
            return new ScreenConfig()
            {
                SuppressPersist = true,
                VisualConfig = new VisualConfig()
                {
                    Padding = new ThicknessInfo(0),
                },
                HeaderConfigs = new List<IViewConfig>()
                {
                    new ViewConfigBuilder<st1.HeaderTitleBarContext>()
                    {
                        component = st1.HeaderTitleBar.COMPONENT_NAME,
                        configuration = new st1.HeaderTitleBarContext()
                        {
                            LeftIcon = FontAwesome.fa_bars,
                            IconFontSize = 20,
                            Title = "Welcome to sample!", // should be this.API.Localize(...)
                            TextColor = AppColors.PrimaryWhite,
                            LeftCommandName = WellKnownCommands.APP_NAVIGATE_PUSH,
                            LeftCommandParameter = WellKnownScreens.ONBOARDING
                        }
                    }
                },
                ViewConfigs = new List<IViewConfig>()
                {
                    new ViewConfigBuilder<st1.H1Context>()
                    {
                        component = st1.H1.COMPONENT_NAME,
                        configuration = new st1.H1Context()
                        {
                            Text = "You have loaded a sample screen. Congratz.",
                            TextColor = AppColors.Accent400
                        }
                    },
                }
            };
        }
#endif
        private string ImageForColorMode(string imageName)
        {
            return base.ExecuteFunction("ImageForColorMode", delegate ()
            {
                if (this.API.Application.DarkMode.GetValueOrDefault())
                {
                    return Path.GetFileNameWithoutExtension(imageName) + "_ondark" + Path.GetExtension(imageName);
                }
                else
                {
                    return Path.GetFileNameWithoutExtension(imageName) + "_onlight" + Path.GetExtension(imageName);
                }
            });
        }
    }
}
