
using System;
using System.Collections.Generic;

namespace Stencil.Forms.Presentation.Menus
{
    public class MenuEntry : PropertyClass, IMenuEntry
    {
        public MenuEntry()
        {

        }

        public string Identifier { get; set; }
        public bool IsIcon { get; set; }
        public string IconCharacter { get; set; }
        public string Label { get; set; }
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
                }
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
    }
}
