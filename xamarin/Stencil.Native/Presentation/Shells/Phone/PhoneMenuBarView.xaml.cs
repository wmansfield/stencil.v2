﻿using Stencil.Native.Base;
using Stencil.Native.Commanding;
using Stencil.Native.Presentation.Menus;
using System;

using Xamarin.Forms;

namespace Stencil.Native.Presentation.Shells.Phone
{
    public partial class PhoneMenuBarView : BaseContentView, IMenuView, IMainMenuView
    {
        public PhoneMenuBarView()
            : base(nameof(PhoneMenuBarView))
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
                if(menuViewModel?.CommandProcessor != null)
                {
                    IMenuEntry entry = (sender as View)?.BindingContext as IMenuEntry;
                    if (!string.IsNullOrWhiteSpace(entry?.CommandName))
                    {
                        CommandScope commandScope = new CommandScope(menuViewModel.CommandProcessor)
                        {
                            TargetMenuEntry = entry
                        };

                        await menuViewModel.CommandProcessor.ExecuteCommandAsync(commandScope, entry.CommandName, entry.CommandParameter);
                    }
                }
                
            });
        }
    }
}