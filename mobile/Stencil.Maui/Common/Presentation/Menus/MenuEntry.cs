
using System;
using System.Collections.Generic;

namespace Stencil.Maui.Presentation.Menus
{
    public class MenuEntry : PropertyClass, IMenuEntry
    {
        public MenuEntry()
        {

        }

        public string Identifier { get; set; }

        private bool _isIcon;
        public bool IsIcon
        {
            get
            {
                return _isIcon;
            }
            set
            {
                if (SetProperty(ref _isIcon, value))
                {
                    this.OnPropertyChanged(nameof(UIShowIcon));
                }
            }
        }
        private string _iconCharacter;
        public string IconCharacter
        {
            get { return _iconCharacter; }
            set { SetProperty(ref _iconCharacter, value); }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        public string CommandName { get; set; }
        public string CommandParameter { get; set; }

        public string ActiveBackgroundColor { get; set; }
        public string ActiveTextColor { get; set; }

        public string SelectedBackgroundColor { get; set; }
        public string SelectedTextColor { get; set; }

        public string UnselectedBackgroundColor { get; set; }
        public string UnselectedTextColor { get; set; }

        public string UITextColor
        {
            get 
            {
                if (this.UIActive)
                {
                    return this.ActiveTextColor;
                }
                else if (this.UISelected)
                {
                    return this.SelectedTextColor;
                }
                else
                {
                    return this.UnselectedTextColor;
                }
            }
        }

        public string UIBackgroundColor
        {
            get
            {
                if (this.UIActive)
                {
                    return this.ActiveBackgroundColor;
                }
                else if (this.UISelected)
                {
                    return this.SelectedBackgroundColor;
                }
                else
                {
                    return this.UnselectedBackgroundColor;
                }
            }
        }


        private bool _uiActive;
        public bool UIActive
        {
            get { return _uiActive; }
            set
            {
                if (SetProperty(ref _uiActive, value))
                {
                    this.OnPropertyChanged(nameof(UIBackgroundColor));
                    this.OnPropertyChanged(nameof(UITextColor));
                    this.OnPropertyChanged(nameof(UIShowIcon));
                }
            }
        }

        private bool _uiActiveSlow;
        public bool UIActiveSlow
        {
            get { return _uiActiveSlow; }
            set
            {
                if (SetProperty(ref _uiActiveSlow, value))
                {
                    this.OnPropertyChanged(nameof(UIShowIcon));
                }
            }
        }


        public bool UIShowIcon
        {
            get 
            { 
                if (!this.IsIcon)
                {
                    return false;
                }
                return !this.UIActiveSlow; 
            }
            set
            {
                
            }
        }

        private bool _uiSelected;
        public bool UISelected
        {
            get { return _uiSelected; }
            set 
            { 
                if(SetProperty(ref _uiSelected, value))
                {
                    this.OnPropertyChanged(nameof(UIBackgroundColor));
                    this.OnPropertyChanged(nameof(UITextColor));
                }
            }
        }

        private bool _uiSuppressed;
        public bool UISuppressed
        {
            get { return _uiSuppressed; }
            set { SetProperty(ref _uiSuppressed, value); }
        }
    }
}
