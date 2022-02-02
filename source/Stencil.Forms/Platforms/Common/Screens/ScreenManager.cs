using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Common.Views;
using Stencil.Forms.Commanding;
using Stencil.Forms.Data;
using Stencil.Forms.Presentation.Menus;
using Stencil.Forms.Resourcing;
using Stencil.Forms.Views;
using Stencil.Forms.Views.Standard;
using Stencil.Forms.Views.Standard.v1_0;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Screens
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

                        IResolvableTemplateSelector resolvableTemplateSelector = result.DataTemplateSelector as IResolvableTemplateSelector;
                        //TODO:MUST: Null IResolvableTemplateSelector


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

                        ObservableCollection<object> headerItems = new ObservableCollection<object>();
                        if (screenConfig.HeaderConfigs != null)
                        {
                            foreach (IViewConfig viewConfig in screenConfig.HeaderConfigs)
                            {
                                IDataViewItem dataViewItem = this.GenerateViewItem(result, viewConfig);
                                if (dataViewItem != null)
                                {
                                    IDataViewComponent viewComponent = await resolvableTemplateSelector.ResolveTemplateAndPrepareDataAsync(dataViewItem);
                                    headerItems.Add(dataViewItem.PreparedContext);
                                }
                            }
                        }

                        ObservableCollection<object> footerItems = new ObservableCollection<object>();
                        if (screenConfig.FooterConfigs != null)
                        {
                            foreach (IViewConfig viewConfig in screenConfig.FooterConfigs)
                            {
                                IDataViewItem dataViewItem = this.GenerateViewItem(result, viewConfig);
                                if (dataViewItem != null)
                                {
                                    IDataViewComponent viewComponent = await resolvableTemplateSelector.ResolveTemplateAndPrepareDataAsync(dataViewItem);
                                    footerItems.Add(dataViewItem.PreparedContext);
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

                        if(resolvableTemplateSelector != null)
                        {
                            await result.ExtractAndPrepareExtensionsAsync();
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


                        await result.Initialize();
                    }
                }

                return result;
            });
        }
        public virtual Task<IDataViewModel> GenerateViewModelAsync(ICommandProcessor commandProcessor, IScreenConfig screenConfig)
        {
            return base.ExecuteFunctionAsync<IDataViewModel>(nameof(GenerateViewModelAsync), async delegate ()
            {
                StandardDataViewModel result = null;

                if (screenConfig != null)
                {
                    result = new StandardDataViewModel(commandProcessor, this.CreateDataTemplateSelector);

                    IResolvableTemplateSelector resolvableTemplateSelector = result.DataTemplateSelector as IResolvableTemplateSelector;
                    //TODO:MUST: Null IResolvableTemplateSelector [not really supported, should we harden the requirement?]

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

                    ObservableCollection<object> headerItems = new ObservableCollection<object>();
                    if (screenConfig.HeaderConfigs != null)
                    {
                        foreach (IViewConfig viewConfig in screenConfig.HeaderConfigs)
                        {
                            IDataViewItem dataViewItem = this.GenerateViewItem(result, viewConfig);
                            if (dataViewItem != null)
                            {
                                IDataViewComponent viewComponent = await resolvableTemplateSelector.ResolveTemplateAndPrepareDataAsync(dataViewItem);
                                headerItems.Add(dataViewItem.PreparedContext);
                            }
                        }
                    }

                    ObservableCollection<object> footerItems = new ObservableCollection<object>();
                    if (screenConfig.FooterConfigs != null)
                    {
                        foreach (IViewConfig viewConfig in screenConfig.FooterConfigs)
                        {
                            IDataViewItem dataViewItem = this.GenerateViewItem(result, viewConfig);
                            if (dataViewItem != null)
                            {
                                IDataViewComponent viewComponent = await resolvableTemplateSelector.ResolveTemplateAndPrepareDataAsync(dataViewItem);
                                footerItems.Add(dataViewItem.PreparedContext);
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

                    if (resolvableTemplateSelector != null)
                    {
                        if (result.MainItemsUnFiltered != null)
                        {
                            foreach (IDataViewItem item in result.MainItemsUnFiltered)
                            {
                                IDataViewComponent viewComponent = await resolvableTemplateSelector.ResolveTemplateAndPrepareDataAsync(item);
                                if (item.PreparedContext is IDataViewFilter dataViewFilter)
                                {
                                    result.Filters.Add(dataViewFilter);
                                }
                                if (item.PreparedContext is IDataViewAdjuster dataViewAdjuster)
                                {
                                    result.Adjusters.Add(dataViewAdjuster);
                                }
                                if (item.PreparedContext is IStateResponder stateResponder)
                                {
                                    result.AddStateResponder(stateResponder);
                                }
                                if (item.PreparedContext is IStateEmitter stateEmitter)
                                {
                                    result.StateEmitters.Add(stateEmitter);
                                }
                            }
                        }
                        if (result.HeaderItems != null)
                        {
                            foreach (object headerItem in result.HeaderItems)
                            {
                                IDataViewItem dataViewItem = headerItem as IDataViewItem;
                                if (dataViewItem == null)
                                {
                                    IDataViewItemReference dataViewItemReference = headerItem as IDataViewItemReference;
                                    if (dataViewItemReference != null)
                                    {
                                        dataViewItem = dataViewItemReference.DataViewItem;
                                    }
                                }
                                if (dataViewItem != null)
                                {
                                    IDataViewComponent viewComponent = await resolvableTemplateSelector.ResolveTemplateAndPrepareDataAsync(dataViewItem);
                                    this.LogTrace($"{viewComponent.GetType()} -> {dataViewItem.PreparedContext.GetType()}");

                                    if (dataViewItem.PreparedContext is IDataViewFilter dataViewFilter)
                                    {
                                        result.Filters.Add(dataViewFilter);
                                    }
                                    if (dataViewItem.PreparedContext is IDataViewAdjuster dataViewAdjuster)
                                    {
                                        result.Adjusters.Add(dataViewAdjuster);
                                    }
                                    if (dataViewItem.PreparedContext is IStateResponder stateResponder)
                                    {
                                        result.AddStateResponder(stateResponder);
                                    }
                                    if (dataViewItem.PreparedContext is IStateEmitter stateEmitter)
                                    {
                                        result.StateEmitters.Add(stateEmitter);
                                    }
                                }
                            }
                        }
                        if (result.FooterItems != null)
                        {
                            foreach (object footerItem in result.FooterItems)
                            {
                                IDataViewItem dataViewItem = footerItem as IDataViewItem;
                                if (dataViewItem == null)
                                {
                                    IDataViewItemReference dataViewItemReference = footerItem as IDataViewItemReference;
                                    if (dataViewItemReference != null)
                                    {
                                        dataViewItem = dataViewItemReference.DataViewItem;
                                    }
                                }
                                if (dataViewItem != null)
                                {
                                    IDataViewComponent viewComponent = await resolvableTemplateSelector.ResolveTemplateAndPrepareDataAsync(dataViewItem);
                                    if (dataViewItem.PreparedContext is IDataViewFilter dataViewFilter)
                                    {
                                        result.Filters.Add(dataViewFilter);
                                    }
                                    if (dataViewItem.PreparedContext is IDataViewAdjuster dataViewAdjuster)
                                    {
                                        result.Adjusters.Add(dataViewAdjuster);
                                    }
                                    if (dataViewItem.PreparedContext is IStateResponder stateResponder)
                                    {
                                        result.AddStateResponder(stateResponder);
                                    }
                                    if (dataViewItem.PreparedContext is IStateEmitter stateEmitter)
                                    {
                                        result.StateEmitters.Add(stateEmitter);
                                    }
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


                    await result.Initialize();
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

                if (viewConfig.encapsulated_views != null)
                {
                    List<IDataViewItem> encapsulatedItems = new List<IDataViewItem>();
                    foreach (IViewConfig childViewConfig in viewConfig.encapsulated_views)
                    {
                        IDataViewItem childViewItem = this.GenerateViewItem(dataViewModel, childViewConfig);
                        if (childViewItem != null)
                        {
                            encapsulatedItems.Add(childViewItem);
                        }
                    }
                    result.EncapsulatedItems = encapsulatedItems.ToArray();
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
                                    await this.API.CommandProcessor.ExecuteCommandAsync(commandScope, item.CommandName, item.CommandParameter, null);
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
