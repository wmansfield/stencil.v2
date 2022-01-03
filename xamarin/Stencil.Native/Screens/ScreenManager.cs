using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Data;
using Stencil.Native.Presentation.Menus;
using Stencil.Native.Resourcing;
using Stencil.Native.Views;
using Stencil.Native.Views.Standard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Native.Screens
{
    public abstract class ScreenManager<TAPI> : TrackedClass<TAPI>, IScreenManager
        where TAPI : StencilAPI
    {
        #region Constructor

        public ScreenManager(TAPI api)
            : base(api, nameof(ScreenManager<TAPI>))
        {
        }

        #endregion


        #region Public Methods

        public virtual Task<IDataViewModel> GenerateViewModelAsync(ICommandProcessor commandProcessor, INavigationData navigationData)
        {
            return base.ExecuteFunctionAsync<IDataViewModel>(nameof(GenerateViewModelAsync), async delegate ()
            {
                StandardDataViewModel result = null;

                if (!string.IsNullOrWhiteSpace(navigationData?.screen_name))
                {
                    IScreenConfig screenConfig = await this.LoadScreenConfigAsync(commandProcessor, navigationData);
                    if (screenConfig != null)
                    {
                        result = new StandardDataViewModel(commandProcessor, this.CreateDataTemplateSelector);

                        // map to view elements
                        ObservableCollection<IDataViewItem> mainItems = new ObservableCollection<IDataViewItem>();
                        if (screenConfig.ViewConfigs != null)
                        {
                            foreach (IViewConfig viewConfig in screenConfig.ViewConfigs)
                            {
                                IDataViewItem dataViewItem = this.GenerateViewItem(result, viewConfig);
                                if (dataViewItem != null)
                                {
                                    mainItems.Add(dataViewItem);
                                }
                            }
                        }

                        ObservableCollection<IDataViewItem> headerItems = new ObservableCollection<IDataViewItem>();
                        if (screenConfig.HeaderConfigs != null)
                        {
                            foreach (IViewConfig viewConfig in screenConfig.HeaderConfigs)
                            {
                                IDataViewItem dataViewItem = this.GenerateViewItem(result, viewConfig);
                                if (dataViewItem != null)
                                {
                                    headerItems.Add(dataViewItem);
                                }
                            }
                        }

                        ObservableCollection<IDataViewItem> footerItems = new ObservableCollection<IDataViewItem>();
                        if (screenConfig.FooterConfigs != null)
                        {
                            foreach (IViewConfig viewConfig in screenConfig.FooterConfigs)
                            {
                                IDataViewItem dataViewItem = this.GenerateViewItem(result, viewConfig);
                                if (dataViewItem != null)
                                {
                                    footerItems.Add(dataViewItem);
                                }
                            }
                        }

                        ObservableCollection<IMenuEntry> menuEntries = null;
                        if (screenConfig.MenuConfigs != null)
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


                        // assign mapped element
                        result.IsMenuSupported = screenConfig.IsMenuSupported;
                        result.MainItemsUnFiltered = mainItems;
                        result.HeaderItems = headerItems;
                        result.ShowHeader = headerItems.Count > 0;
                        result.FooterItems = footerItems;
                        result.ShowFooter = footerItems.Count > 0;
                        result.MenuEntries = menuEntries;
                        result.ShowCommands = screenConfig.ShowCommands;


                        // extract filters or augmentations

                        IResolvableTemplateSelector resolvableTemplateSelector = result.DataTemplateSelector as IResolvableTemplateSelector;
                        if(resolvableTemplateSelector != null)
                        {
                            if (result.MainItemsUnFiltered != null)
                            {
                                foreach (IDataViewItem item in result.MainItemsUnFiltered)
                                {
                                    IDataViewComponent viewComponent = resolvableTemplateSelector.ResolveTemplateAndPrepareData(item);
                                    if (item.PreparedData is IDataViewFilter dataViewFilter)
                                    {
                                        result.Filters.Add(dataViewFilter);
                                    }
                                    if (item.PreparedData is IDataViewAdjuster dataViewAdjuster)
                                    {
                                        result.Adjusters.Add(dataViewAdjuster);
                                    }
                                }
                            }
                            if (result.HeaderItems != null)
                            {
                                foreach (IDataViewItem item in result.HeaderItems)
                                {
                                    IDataViewComponent viewComponent = resolvableTemplateSelector.ResolveTemplateAndPrepareData(item);
                                    if (item.PreparedData is IDataViewFilter dataViewFilter)
                                    {
                                        result.Filters.Add(dataViewFilter);
                                    }
                                    if (item.PreparedData is IDataViewAdjuster dataViewAdjuster)
                                    {
                                        result.Adjusters.Add(dataViewAdjuster);
                                    }
                                }
                            }
                            if (result.FooterItems != null)
                            {
                                foreach (IDataViewItem item in result.FooterItems)
                                {
                                    IDataViewComponent viewComponent = resolvableTemplateSelector.ResolveTemplateAndPrepareData(item);
                                    if (item.PreparedData is IDataViewFilter dataViewFilter)
                                    {
                                        result.Filters.Add(dataViewFilter);
                                    }
                                    if (item.PreparedData is IDataViewAdjuster dataViewAdjuster)
                                    {
                                        result.Adjusters.Add(dataViewAdjuster);
                                    }
                                }
                            }
                        }
                        

                        // apply page visuals
                        if (screenConfig.VisualConfig != null)
                        {
                            result.BackgroundImage = screenConfig.VisualConfig.BackgroundImage;

                            if (!string.IsNullOrWhiteSpace(screenConfig.VisualConfig.BackgroundColor))
                            {
                                result.BackgroundColor = Color.FromHex(screenConfig.VisualConfig.BackgroundColor);
                            }
                            result.Padding = screenConfig.VisualConfig.Padding.ToThickness();
                        }


                        await result.InitializeData();
                    }
                }

                return result;
            });
        }

        public virtual IDataViewItem GenerateViewItem(IDataViewModel dataViewModel, IViewConfig viewConfig)
        {
            return base.ExecuteFunction(nameof(GenerateViewItem), delegate ()
            {
                if (string.IsNullOrWhiteSpace(viewConfig.component))
                {
                    return null;
                }
                StandardDataViewItem result = new StandardDataViewItem()
                {
                    DataViewModel = dataViewModel,
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
                                IDataViewItem childViewItem = this.GenerateViewItem(dataViewModel, item);
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

        public virtual Task RemoveScreenConfigAsync(string screenStorageKey)
        {
            return base.ExecuteFunction(nameof(RemoveScreenConfigAsync), delegate ()
            {
                using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                {
                    database.ScreenConfig_Remove(screenStorageKey);
                }
                return Task.CompletedTask;
            });
        }
        public virtual Task InvalidateScreenConfigAsync(string screenStorageKey)
        {
            return base.ExecuteFunction(nameof(InvalidateScreenConfigAsync), delegate ()
            {
                using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                {
                    database.ScreenConfig_Invalidate(screenStorageKey);
                }
                return Task.CompletedTask;
            });
        }
        public virtual Task<ScreenConfig> RetrieveScreenConfigAsync(string screenStorageKey, bool includeExpired)
        {
            return base.ExecuteFunction(nameof(RetrieveScreenConfigAsync), delegate ()
            {
                ScreenConfig result = null;
                using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                {
                    result = database.ScreenConfig_Get(screenStorageKey);
                }
                if(!includeExpired)
                {
                    if(result != null)
                    {
                        if (result.ExpireUTC.HasValue && result.ExpireUTC.Value < DateTimeOffset.Now)
                        {
                            result = null; //TODO:MUST: How do we clean expired items?
                        }
                    }
                }
                
                return Task.FromResult(result);
            });
        }

        public virtual Task<List<ScreenConfig>> GetScreenConfigsWithNameAsync(string screenName)
        {
            return base.ExecuteFunction(nameof(GetScreenConfigsWithNameAsync), delegate ()
            {
                List<ScreenConfig> result = null;
                using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                {
                    result = database.ScreenConfig_GetWithName(screenName);
                }
                return Task.FromResult(result);
            });
        }

        public virtual Task SaveScreenConfigAsync(ScreenConfig screenConfig)
        {
            return base.ExecuteFunction(nameof(SaveScreenConfigAsync), delegate ()
            {
                if(string.IsNullOrWhiteSpace(screenConfig.ScreenStorageKey))
                {
                    screenConfig.ScreenStorageKey = ScreenConfig.FormatStorageKey(screenConfig.ScreenName, screenConfig.ScreenParameter);
                }
                using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                {
                    database.ScreenConfig_Upsert(screenConfig);
                }
                return Task.CompletedTask;
            });
        }


        public virtual List<IScreenConfig> GetForDownloading()
        {
            return base.ExecuteFunction(nameof(GetForDownloading), delegate ()
            {
                List<IScreenConfig> result = new List<IScreenConfig>();
                List<ScreenConfig> screenConfigs = null;

                using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                {
                    screenConfigs = database.ScreenConfig_GetForDownloading();
                }

                if(screenConfigs != null)
                {
                    foreach (ScreenConfig item in screenConfigs)
                    {
                        result.Add(item);
                    }
                }

                return result;
            });
        }

        #endregion

        #region Protected Methods

        protected virtual DataTemplateSelector CreateDataTemplateSelector(ICommandScope scope)
        {
            return base.ExecuteFunction(nameof(CreateDataTemplateSelector), delegate ()
            {
                return new ComponentLibraryTemplateSelector(scope);
            });
        }

        protected IMenuEntry GenerateMenuItem(IMenuConfig viewConfig)
        {
            return base.ExecuteFunction(nameof(GenerateMenuItem), delegate ()
            {
                if (string.IsNullOrWhiteSpace(viewConfig.label) && string.IsNullOrWhiteSpace(viewConfig.icon_character))
                {
                    return null;
                }
                MenuEntry result = new MenuEntry()
                {
                    Identifier = viewConfig.identifier,
                    UISelected = viewConfig.is_selected,
                    Label = viewConfig.label,
                    IconCharacter = viewConfig.icon_character,
                    IsIcon = viewConfig.is_icon,
                    CommandName = viewConfig.command,
                    CommandParameter = viewConfig.command_parameter,
                    SelectedBackgroundColor = AppColors.MenuSelectedBackground,
                    UnselectedBackgroundColor = AppColors.MenuUnselectedBackground,
                    SelectedTextColor = AppColors.MenuSelectedText,
                    UnselectedTextColor = AppColors.MenuUnselectedText,
                    ActiveBackgroundColor = AppColors.MenuActiveBackground,
                    ActiveTextColor = AppColors.MenuActiveText
                };

                return result;
            });
        }

        protected Task<IScreenConfig> LoadScreenConfigAsync(ICommandProcessor commandProcessor, INavigationData navigationData)
        {
            return base.ExecuteFunctionAsync(nameof(LoadScreenConfigAsync), async delegate ()
            {
                ScreenConfig result = null;

                if (!string.IsNullOrWhiteSpace(navigationData?.screen_name))
                {
                    string screenStorageKey = ScreenConfig.FormatStorageKey(navigationData.screen_name, navigationData.screen_parameter);

                    using (IStencilDatabase database = this.API.StencilDatabase.OpenStencilDatabase())
                    {
                        result = database.ScreenConfig_Get(screenStorageKey);
                    }
                }

                if (result == null)
                {
                    // manually build if missing from database
                    result = await this.GenerateMissingScreenConfigAsync(navigationData);
                    
                    if(result != null)
                    {
                        if (result.DownloadCommands?.Count > 0)
                        {
                            CommandScope commandScope = new CommandScope(commandProcessor);

                            foreach (ICommandConfig item in result.DownloadCommands)
                            {
                                try
                                {
                                    await this.API.CommandProcessor.ExecuteCommandAsync(commandScope, item.CommandName, item.CommandParameter);
                                }
                                catch (Exception ex)
                                {
                                    this.LogError(ex, string.Format("ProcessScreenJob.DownloadCommand:{0}:{1}" + item.CommandName, item.CommandParameter));
                                }
                            }
                        }

                        if (!result.SuppressPersist)
                        {
                            // ensure we have an id, in case implementor forgot
                            if (string.IsNullOrWhiteSpace(result.ScreenStorageKey))
                            {
                                result.ScreenStorageKey = ScreenConfig.FormatStorageKey(navigationData.screen_name, navigationData.screen_parameter);
                            }
                            if (!result.DownloadedUTC.HasValue)
                            {
                                result.DownloadedUTC = DateTimeOffset.UtcNow;
                            }
                            if (result.ScreenNavigationData != null)
                            {
                                result.ScreenNavigationData.last_retrieved_utc = DateTime.UtcNow;
                            }

                            await this.SaveScreenConfigAsync(result);
                        }
                    }
                }

                IScreenConfig screenConfig = result as IScreenConfig;
                if (screenConfig != null)
                {
                    screenConfig = await this.PostProcessScreenConfigAsync(screenConfig);
                }

                return screenConfig;
            });
        }

        #endregion

        #region Abstract Methods

        protected abstract Task<ScreenConfig> GenerateMissingScreenConfigAsync(INavigationData navigationData);

        protected abstract Task<IScreenConfig> PostProcessScreenConfigAsync(IScreenConfig screenConfig);

        
        #endregion
    }
}
