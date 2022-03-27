using Newtonsoft.Json;
using Stencil.Forms.Base;
using Stencil.Forms.Commanding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard
{
    public class StandardNestedDataViewModel : BaseViewModel, INestedDataViewModel
    {
        public StandardNestedDataViewModel(ICommandProcessor commandProcessor, Func<ICommandScope, DataTemplateSelector> dataTemplateSelectorCreator)
            : this(nameof(StandardNestedDataViewModel), commandProcessor, dataTemplateSelectorCreator)
        {
        }

        public StandardNestedDataViewModel(ICommandProcessor commandProcessor, DataTemplateSelector dataTemplateSelector)
            : this(nameof(StandardNestedDataViewModel), commandProcessor, dataTemplateSelector)
        {
        }

        public StandardNestedDataViewModel(string trackPrefix, ICommandProcessor commandProcessor, Func<ICommandScope, DataTemplateSelector> dataTemplateSelectorCreator)
            : base(trackPrefix)
        {
            this.ResetState();

            this.CommandScope = new CommandScope(commandProcessor);
            this.DataTemplateSelector = dataTemplateSelectorCreator(this.CommandScope);
        }

        public StandardNestedDataViewModel(string trackPrefix, ICommandProcessor commandProcessor, DataTemplateSelector dataTemplateSelector)
            : base(trackPrefix)
        {
            this.ResetState();

            this.CommandScope = new CommandScope(commandProcessor);
            this.DataTemplateSelector = dataTemplateSelector;
        }

        public virtual ICommandScope CommandScope { get; set; }
        public virtual IDataViewVisual DataViewVisual { get; set; }
        public virtual DataTemplateSelector DataTemplateSelector { get; set; }

        private ObservableCollection<IDataViewItem> _mainItemsUnFiltered;
        public virtual ObservableCollection<IDataViewItem> MainItemsUnFiltered
        {
            get { return _mainItemsUnFiltered; }
            set { SetProperty(ref _mainItemsUnFiltered, value); }
        }


        private ObservableCollection<object> _mainItemsFiltered;
        public virtual ObservableCollection<object> MainItemsFiltered
        {
            get { return _mainItemsFiltered; }
            set { SetProperty(ref _mainItemsFiltered, value); }
        }


        private Thickness _padding;
        public virtual Thickness Padding
        {
            get { return _padding; }
            set { SetProperty(ref _padding, value); }
        }

        private Color _backgroundColor;
        public virtual Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private string _backgroundImage;
        public virtual string BackgroundImage
        {
            get { return _backgroundImage; }
            set { SetProperty(ref _backgroundImage, value); }
        }

        private ObservableCollection<object> _headerItems;
        public virtual ObservableCollection<object> HeaderItems
        {
            get { return _headerItems; }
            set { SetProperty(ref _headerItems, value); }
        }

        private bool _showHeader;
        public virtual bool ShowHeader
        {
            get { return _showHeader; }
            set { SetProperty(ref _showHeader, value); }
        }

        private ObservableCollection<object> _footerItems;
        public virtual ObservableCollection<object> FooterItems
        {
            get { return _footerItems; }
            set { SetProperty(ref _footerItems, value); }
        }

        private bool _showFooter;
        public virtual bool ShowFooter
        {
            get { return _showFooter; }
            set { SetProperty(ref _showFooter, value); }
        }

        private Brush _backgroundBrush;
        public virtual Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
            set { SetProperty(ref _backgroundBrush, value); }
        }

        public virtual List<IDataViewFilter> Filters { get; set; }
        public virtual List<IDataViewAdjuster> Adjusters { get; set; }
        public virtual Dictionary<string, List<IStateResponder>> StateResponders { get; set; }
        public virtual List<IStateEmitter> StateEmitters { get; set; }

        public virtual HashSet<string> Claims { get; set; }

        public virtual Task Initialize()
        {
            return base.ExecuteMethodAsync(nameof(Initialize), async delegate ()
            {
                this.PrepareInteractionDefaultValues();

                await this.ApplyFiltersAndAdjustmentsAsync();
            });
        }

        public virtual Task ApplyFiltersAndAdjustmentsAsync()
        {
            return base.ExecuteMethodAsync(nameof(ApplyFiltersAndAdjustmentsAsync), async delegate ()
            {
                List<IDataViewItem> filteredItems = new List<IDataViewItem>();
                ObservableCollection<IDataViewItem> unFilteredItems = this.MainItemsUnFiltered;

                if (unFilteredItems?.Count > 0)
                {
                    if (this.Filters?.Count > 0)
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
                            if (!shouldSuppress)
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

                if (this.Adjusters?.Count > 0)
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

        public virtual void AddStateResponder(IStateResponder stateResponder)
        {
            base.ExecuteMethod(nameof(AddStateResponder), delegate ()
            {
                string[] interactionGroups = stateResponder?.GetInteractionGroups();
                if (interactionGroups?.Length > 0)
                {
                    foreach (string group in interactionGroups)
                    {
                        if(!this.StateResponders.ContainsKey(group))
                        {
                            this.StateResponders[group] = new List<IStateResponder>();
                        }
                        this.StateResponders[group].Add(stateResponder);
                    }
                }
            });
        }

        public virtual void RaiseStateChange(string group, string name, string state)
        {
            base.ExecuteMethod(nameof(RaiseStateChange), delegate ()
            {
                if(this.StateResponders == null || group == null || name == null)
                {
                    return;
                }
                List<IStateResponder> responders = null;
                if (this.StateResponders.TryGetValue(group, out responders) && responders != null)
                {
                    foreach (IStateResponder stateResponder in responders)
                    {
                        try
                        {
                            stateResponder.OnInteractionStateChanged(group, name, state);
                        }
                        catch (Exception ex)
                        {
                            this.LogError(ex, $"{this.TrackPrefix}.{nameof(RaiseStateChange)}");
                        }
                    }
                }
            });
        }

        public virtual void PrepareInteractionDefaultValues()
        {
            base.ExecuteMethod(nameof(PrepareInteractionDefaultValues), delegate ()
            {
                if(this.StateEmitters != null)
                {
                    foreach (IStateEmitter item in this.StateEmitters)
                    {
                        try
                        {
                            item.EmitDefaultState(this);
                        }
                        catch (Exception ex)
                        {
                            this.LogError(ex, $"Error emitting default values for '{item.GetType()}'");
                        }
                    }
                }
            });
        }

        public virtual Task ExtractAndPrepareExtensionsAsync()
        {
            return base.ExecuteMethodAsync(nameof(ExtractAndPrepareExtensionsAsync), async delegate ()
            {
                if (this.MainItemsUnFiltered != null)
                {
                    foreach (IDataViewItem item in this.MainItemsUnFiltered)
                    {
                        await this.ExtractAndPrepareExtensionsAsync(item);
                    }
                }

                if (this.HeaderItems != null)
                {
                    foreach (object headerItem in this.HeaderItems)
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
                            await this.ExtractAndPrepareExtensionsAsync(dataViewItem);
                        }
                    }
                }
                if (this.FooterItems != null)
                {
                    foreach (object footerItem in this.FooterItems)
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
                            await this.ExtractAndPrepareExtensionsAsync(dataViewItem);
                        }
                    }
                }
            });

        }

        public virtual Task ExtractAndPrepareExtensionsAsync(IDataViewItem item)
        {
            return base.ExecuteMethodAsync(nameof(ExtractAndPrepareExtensionsAsync), async delegate ()
            {
                if (item == null)
                {
                    return;
                }

                IResolvableTemplateSelector selector = this.DataTemplateSelector as IResolvableTemplateSelector;
                if (selector == null)
                {
                    this.LogError(new Exception("Unable to cast dataTemplateSelector to IResolvableTemplateSelector"), "ExtractAndPrepareExtensionsAsync");
                    return;
                }

                IDataViewComponent viewComponent = await selector.ResolveTemplateAndPrepareDataAsync(item);

                if (item.PreparedContext is IDataViewFilter dataViewFilter)
                {
                    this.Filters.Add(dataViewFilter);
                }
                if (item.PreparedContext is IDataViewAdjuster dataViewAdjuster)
                {
                    this.Adjusters.Add(dataViewAdjuster);
                }
                if (item.PreparedContext is IStateResponder stateResponder)
                {
                    this.AddStateResponder(stateResponder);
                }
                if (item.PreparedContext is IStateEmitter stateEmitter)
                {
                    this.StateEmitters.Add(stateEmitter);
                }
            });
            
        }

        public virtual void ResetState()
        {
            base.ExecuteMethod(nameof(ResetState), delegate ()
            {
                this.Adjusters = new List<IDataViewAdjuster>();
                this.Filters = new List<IDataViewFilter>();
                this.StateEmitters = new List<IStateEmitter>();
                this.StateResponders = new Dictionary<string, List<IStateResponder>>(StringComparer.OrdinalIgnoreCase);
                this.Claims = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            });
        }
    }
}

