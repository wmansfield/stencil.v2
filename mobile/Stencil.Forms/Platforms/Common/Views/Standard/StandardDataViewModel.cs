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
    public class StandardDataViewModel : StandardNestedDataViewModel, IDataViewModel
    {
        public StandardDataViewModel(ICommandProcessor commandProcessor, Func<ICommandScope, DataTemplateSelector> dataTemplateSelectorCreator)
            : base(nameof(StandardDataViewModel), commandProcessor, dataTemplateSelectorCreator)
        {
            
        }

        public StandardDataViewModel(ICommandProcessor commandProcessor, DataTemplateSelector dataTemplateSelector)
            : base(nameof(StandardDataViewModel), commandProcessor, dataTemplateSelector)
        {
            
        }
        
        public bool IsMenuSupported { get; set; }

        public List<ICommandConfig> BeforeShowCommands { get; set; }
        public List<ICommandConfig> AfterShowCommands { get; set; }

        
        private ObservableCollection<IMenuEntry> _menuEntries;
        public ObservableCollection<IMenuEntry> MenuEntries
        {
            get { return _menuEntries; }
            set { SetProperty(ref _menuEntries, value); }
        }

      
        public override Task OnNavigatingToAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnNavigatingToAsync), async delegate ()
            {
                await base.OnNavigatingToAsync();
                if(this.BeforeShowCommands != null)
                {
                    foreach (ICommandConfig showCommand in this.BeforeShowCommands)
                    {
                        await this.API.CommandProcessor.ExecuteCommandAsync(this.CommandScope, showCommand.CommandName, showCommand.CommandParameter, this);
                    }
                }
            });
        }
        public override Task OnNavigatedToAsync()
        {
            return base.ExecuteMethodAsync(nameof(OnNavigatedToAsync), async delegate ()
            {
                await base.OnNavigatedToAsync();
                if (this.AfterShowCommands != null)
                {
                    foreach (ICommandConfig showCommand in this.AfterShowCommands)
                    {
                        await this.API.CommandProcessor.ExecuteCommandAsync(this.CommandScope, showCommand.CommandName, showCommand.CommandParameter, this);
                    }
                }
            });
        }
    }
}
