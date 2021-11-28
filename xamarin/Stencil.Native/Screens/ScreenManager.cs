using Stencil.Native.Commanding;
using Stencil.Native.Data;
using Stencil.Native.Presentation.Menus;
using Stencil.Native.Views;
using Stencil.Native.Views.Standard;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using v1_0 = Stencil.Native.Views.Standard.v1_0;
using v1_1 = Stencil.Native.Views.Standard.v1_1;

namespace Stencil.Native.Screens
{
    public abstract class ScreenManager<TAPI> : TrackedClass<TAPI>, IScreenManager
        where TAPI : StencilAPI
    {
        public ScreenManager(TAPI api)
            : base(api, nameof(ScreenManager<TAPI>))
        {

        }

        public Task<IDataViewModel> GenerateScreenAsync(ICommandProcessor commandProcessor, INavigationData navigationData)
        {
            return base.ExecuteFunctionAsync(nameof(GenerateScreenAsync), async delegate ()
            {
                IDataViewModel result = null;

                if (!string.IsNullOrWhiteSpace(navigationData?.screen))
                {
                    IScreenConfig screenConfig = await this.LoadScreenConfigAsync(navigationData);
                    if(screenConfig != null)
                    {
                        // map to view elements
                        ObservableCollection<IDataViewItem> viewItems = new ObservableCollection<IDataViewItem>();
                        if(screenConfig.ViewConfigs != null)
                        {
                            foreach (IViewConfig viewConfig in screenConfig.ViewConfigs)
                            {
                                IDataViewItem dataViewItem = this.GenerateViewItem(viewConfig);
                                if(dataViewItem != null)
                                {
                                    viewItems.Add(dataViewItem);
                                }
                            }
                        }


                        ObservableCollection<IMenuEntry> menuEntries = null;
                        if(screenConfig.MenuConfigs != null)
                        {
                            menuEntries = new ObservableCollection<IMenuEntry>();
                            foreach (IMenuConfig menuConfig in screenConfig.MenuConfigs)
                            {
                                IMenuEntry entry = this.GenerateMenuItem(menuConfig);
                                if (entry != null)
                                {
                                    menuEntries.Add(entry);
                                }
                            }
                        }

                        // build the view and return it
                        result = new StandardDataViewModel(commandProcessor)
                        {
                            IsMenuSupported = screenConfig.IsMenuSupported,
                            Margin = screenConfig.Margin,
                            BackgroundColor = screenConfig.BackgroundColor,
                            DataViewItems = viewItems,
                            MenuEntries = menuEntries
                        };
                    }
                }

                return result;
            });
        }
        public IDataViewItem GenerateViewItem(IViewConfig viewConfig)
        {
            return base.ExecuteFunction(nameof(GenerateViewItem), delegate ()
            {
                if(string.IsNullOrWhiteSpace(viewConfig.component))
                {
                    return null;
                }
                StandardDataViewItem result = new StandardDataViewItem()
                {
                    Library = viewConfig.library,
                    Component = viewConfig.component,
                    ConfigurationJson = viewConfig.configuration_json,
                };
                if (viewConfig.sections != null)
                {
                    List<IDataViewSection> sections = new List<IDataViewSection>();
                    foreach (SectionConfig section in viewConfig.sections)
                    {
                        StandardDataViewSection dataSection = new StandardDataViewSection();
                        if (section.ViewConfigs != null)
                        {
                            List<IDataViewItem> children = new List<IDataViewItem>();

                            foreach (IViewConfig item in section.ViewConfigs)
                            {
                                IDataViewItem childViewItem = this.GenerateViewItem(item);
                                if (childViewItem != null)
                                {
                                    children.Add(childViewItem);
                                }
                            }

                            dataSection.ViewItems = children.ToArray();
                        }
                        sections.Add(dataSection);
                    }
                    result.Sections = sections.ToArray();
                }

                return result;
            });
        }
        public IMenuEntry GenerateMenuItem(IMenuConfig viewConfig)
        {
            return base.ExecuteFunction(nameof(GenerateMenuItem), delegate ()
            {
                if (string.IsNullOrWhiteSpace(viewConfig.label) && string.IsNullOrWhiteSpace(viewConfig.icon_character))
                {
                    return null;
                }
                MenuEntry result = new MenuEntry()
                {
                    Label = viewConfig.label,
                    IconCharacter = viewConfig.icon_character,
                    IsIcon = viewConfig.is_icon,
                    CommandName = viewConfig.command,
                    CommandParameter = viewConfig.command_parameter
                };

                return result;
            });
        }

        protected Task<IScreenConfig> LoadScreenConfigAsync(INavigationData navigationData)
        {
            return base.ExecuteFunctionAsync(nameof(LoadScreenConfigAsync), async delegate ()
            {
                ScreenConfig result = null;

                if (!string.IsNullOrWhiteSpace(navigationData?.screen))
                {
                    string screenName = string.Format("{0}.{1}", navigationData.screen, navigationData.identifier);

                    using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                    {
                        result = database.ScreenConfig_Get(screenName);
                    }
                }

                if (result == null)
                {
                    // manually build if missing from database
                    result = await this.GenerateMissingScreenConfigAsync(navigationData);

                    if (result != null && !result.SuppressPersist)
                    {
                        if(string.IsNullOrWhiteSpace(result.id))
                        {
                            result.id = string.Format("{0}.{1}", navigationData.screen, navigationData.identifier); // jic
                        }

                        using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                        {
                            database.ScreenConfig_Upsert(result.id, result);
                        }
                    }
                }

                return result as IScreenConfig;
            });
        }

        protected abstract Task<ScreenConfig> GenerateMissingScreenConfigAsync(INavigationData navigationData);
    }
}
