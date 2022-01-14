using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Forms.Base;
using Stencil.Forms.Commanding;
using Stencil.Forms.Presentation.Menus;
using Stencil.Forms.Screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard
{
    public class StandardDataViewModel : BaseViewModel, IDataViewModel
    {
        public StandardDataViewModel(ICommandProcessor commandProcessor, Func<ICommandScope, DataTemplateSelector> dataTemplateSelectorCreator)
            : base(nameof(StandardDataViewModel))
        {
            this.Adjusters = new List<IDataViewAdjuster>();
            this.Filters = new List<IDataViewFilter>();
            this.CommandScope = new CommandScope(commandProcessor);
            this.DataTemplateSelector = dataTemplateSelectorCreator(this.CommandScope);
        }

        public StandardDataViewModel(ICommandProcessor commandProcessor, DataTemplateSelector dataTemplateSelector)
            : base(nameof(StandardDataViewModel))
        {
            this.Adjusters = new List<IDataViewAdjuster>();
            this.Filters = new List<IDataViewFilter>();
            this.CommandScope = new CommandScope(commandProcessor);
            this.DataTemplateSelector = dataTemplateSelector;
        }

        public ICommandScope CommandScope { get; set; }
        public IDataViewVisual DataViewVisual { get; set; }
        public bool IsMenuSupported { get; set; }
        public DataTemplateSelector DataTemplateSelector { get; set; }

        public List<ICommandConfig> ShowCommands { get; set; }

        private ObservableCollection<IDataViewItem> _mainItemsUnFiltered;
        public ObservableCollection<IDataViewItem> MainItemsUnFiltered
        {
            get { return _mainItemsUnFiltered; }
            set { SetProperty(ref _mainItemsUnFiltered, value); }
        }


        private ObservableCollection<object> _mainItemsFiltered;
        public ObservableCollection<object> MainItemsFiltered
        {
            get { return _mainItemsFiltered; }
            set { SetProperty(ref _mainItemsFiltered, value); }
        }

        private ObservableCollection<IMenuEntry> _menuEntries;
        public ObservableCollection<IMenuEntry> MenuEntries
        {
            get { return _menuEntries; }
            set { SetProperty(ref _menuEntries, value); }
        }

        private Thickness _padding;
        public Thickness Padding
        {
            get { return _padding; }
            set { SetProperty(ref _padding, value); }
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private string _backgroundImage;
        public string BackgroundImage
        {
            get { return _backgroundImage; }
            set { SetProperty(ref _backgroundImage, value); }
        }

        private ObservableCollection<object> _headerItems;
        public ObservableCollection<object> HeaderItems
        {
            get { return _headerItems; }
            set { SetProperty(ref _headerItems, value); }
        }

        private bool _showHeader;
        public bool ShowHeader
        {
            get { return _showHeader; }
            set { SetProperty(ref _showHeader, value); }
        }

        private ObservableCollection<object> _footerItems;
        public ObservableCollection<object> FooterItems
        {
            get { return _footerItems; }
            set { SetProperty(ref _footerItems, value); }
        }

        private bool _showFooter;
        public bool ShowFooter
        {
            get { return _showFooter; }
            set { SetProperty(ref _showFooter, value); }
        }

        public List<IDataViewFilter> Filters { get; set; }
        public List<IDataViewAdjuster> Adjusters { get; set; }


        public Task InitializeData()
        {
            return base.ExecuteMethodAsync(nameof(InitializeData), async delegate ()
            {
                await this.ApplyFiltersAndAdjustmentsAsync();
            });
        }

        public Task ApplyFiltersAndAdjustmentsAsync()
        {
            return base.ExecuteMethodAsync(nameof(ApplyFiltersAndAdjustmentsAsync), async delegate ()
            {
                List<IDataViewItem> filteredItems = new List<IDataViewItem>();
                ObservableCollection<IDataViewItem> unFilteredItems = this.MainItemsUnFiltered;
                
                if (unFilteredItems?.Count > 0)
                {
                    if(this.Filters?.Count > 0)
                    {
                        foreach (IDataViewItem item in unFilteredItems)
                        {
                            bool shouldSuppress = false;
                            foreach (IDataViewFilter filter in this.Filters)
                            {
                                try
                                {
                                    shouldSuppress = await filter.ShouldSuppressItem(this, item, filteredItems);
                                    if (shouldSuppress)
                                    {
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    this.LogError(ex, $"Error applying filter with '{filter.GetType()}' against item '{JsonConvert.SerializeObject(item.PreparedContext)}'");
                                }
                            }
                            if(!shouldSuppress)
                            {
                                filteredItems.Add(item);
                            }
                        }
                    }
                    else
                    {
                        filteredItems.AddRange(unFilteredItems);
                    }
                }

                if(this.Adjusters?.Count > 0)
                {
                    foreach (IDataViewAdjuster adjuster in this.Adjusters)
                    {
                        try
                        {
                            await adjuster.AdjustItems(this, filteredItems);
                        }
                        catch (Exception ex)
                        {
                            this.LogError(ex, $"Error applying adjustment with '{adjuster.GetType()}'");
                        }
                    }
                }
                this.LogTrace($"Count is {filteredItems.Count}");
                this.MainItemsFiltered = new ObservableCollection<object>(filteredItems.Select(x => x.PreparedContext));
            });
        }

        public override Task OnNavigatingToAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnNavigatingToAsync), async delegate ()
            {
                await base.OnNavigatingToAsync();
                if(this.ShowCommands != null)
                {
                    foreach (ICommandConfig showCommand in this.ShowCommands)
                    {
                        await this.API.CommandProcessor.ExecuteCommandAsync(this.CommandScope, showCommand.CommandName, showCommand.CommandParameter);
                    }
                }
            });
        }
    }
}
