﻿using Stencil.Native.Base;
using Stencil.Native.Commanding;
using Stencil.Native.Presentation.Menus;
using Stencil.Native.Screens;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard
{
    public class StandardDataViewModel : BaseViewModel, IDataViewModel
    {
        public StandardDataViewModel(ICommandProcessor commandProcessor, DataTemplateSelector dataTemplateSelector = null)
            : base(nameof(StandardDataViewModel))
        {
            this.CommandScope = new CommandScope(commandProcessor);
            this.DataTemplateSelector = dataTemplateSelector;
            if (this.DataTemplateSelector == null)
            {
                this.DataTemplateSelector = new ComponentLibraryTemplateSelector(this.CommandScope);
            }
        }

        public ICommandScope CommandScope { get; set; }
        public IDataViewVisual DataViewVisual { get; set; }
        public bool IsMenuSupported { get; set; }
        public DataTemplateSelector DataTemplateSelector { get; set; }

        public List<ICommandConfig> ShowCommands { get; set; }

        private ObservableCollection<IDataViewItem> _mainItems;
        public ObservableCollection<IDataViewItem> MainItems
        {
            get { return _mainItems; }
            set { SetProperty(ref _mainItems, value); }
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

        private ObservableCollection<IDataViewItem> _footerItems;
        public ObservableCollection<IDataViewItem> FooterItems
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
