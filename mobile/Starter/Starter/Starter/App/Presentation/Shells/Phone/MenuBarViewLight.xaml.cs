using Stencil.Maui.Base;
using Stencil.Maui.Commanding;
using Stencil.Maui.Presentation.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Starter.App.Presentation.Shells.Phone
{
    public partial class MenuBarViewLight : BaseContentView, IMenuView, IMainMenuView
    {
        public MenuBarViewLight()
            : base(nameof(MenuBarViewLight))
        {
            InitializeComponent();
        }

        public IMenuViewModel MenuViewModel
        {
            get
            {
                return this.BindingContext as IMenuViewModel;
            }
            set
            {
                this.BindingContext = value;
            }
        }

        public View GetSelf()
        {
            return this;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await base.ExecuteMethodAsync(nameof(TapGestureRecognizer_Tapped), async delegate ()
            {
                IMenuViewModel menuViewModel = this.MenuViewModel;
                if (menuViewModel?.CommandProcessor != null)
                {
                    IMenuEntry entry = (sender as View)?.BindingContext as IMenuEntry;
                    if (!string.IsNullOrWhiteSpace(entry?.CommandName))
                    {
                        CommandScope commandScope = new CommandScope(menuViewModel.CommandProcessor)
                        {
                            TargetMenuEntry = entry
                        };
                        try
                        {
                            entry.UIActive = true;

                            await menuViewModel.CommandProcessor.ExecuteCommandAsync(commandScope, entry.CommandName, entry.CommandParameter, null);
                        }
                        finally
                        {
                            entry.UIActive = false;
                        }
                    }
                }

            });
        }

        private async void PrimaryButton_Tapped(object sender, EventArgs e)
        {
            await base.ExecuteMethodAsync(nameof(PrimaryButton_Tapped), async delegate ()
            {
                IMenuViewModel menuViewModel = this.MenuViewModel;
                if (menuViewModel?.CommandProcessor != null)
                {
                    IMenuEntry entry = menuViewModel.MenuEntries.FirstOrDefault(x => x.UISuppressed);
                    if (!string.IsNullOrWhiteSpace(entry?.CommandName))
                    {
                        CommandScope commandScope = new CommandScope(menuViewModel.CommandProcessor)
                        {
                            TargetMenuEntry = entry
                        };
                        try
                        {
                            entry.UIActive = true;

                            await menuViewModel.CommandProcessor.ExecuteCommandAsync(commandScope, entry.CommandName, entry.CommandParameter, null);
                        }
                        finally
                        {
                            entry.UIActive = false;
                        }
                    }
                }

            });
        }
    }
}