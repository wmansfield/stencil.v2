using Stencil.Forms.Base;
using Stencil.Forms.Commanding;
using System.Collections.Generic;

namespace Stencil.Forms.Presentation.Menus
{
    public class MainMenuViewModel : BaseViewModel, IMenuViewModel
    {
        public MainMenuViewModel()
            : base(nameof(MainMenuViewModel))
        {
            this.MenuEntries = new List<IMenuEntry>();
        }
        public IList<IMenuEntry> MenuEntries { get; set; }

        public ICommandProcessor CommandProcessor { get; set; }


        private string _selectedIdentifier;
        public string SelectedIdentifier
        {
            get { return _selectedIdentifier; }
            set 
            { 
                SetProperty(ref _selectedIdentifier, value);
                this.SyncSelectedMenuItem();
            }
        }

        protected void SyncSelectedMenuItem()
        {
            base.ExecuteMethod(nameof(SyncSelectedMenuItem), delegate ()
            {
                IList<IMenuEntry> entries = this.MenuEntries;
                if(entries != null)
                {
                    for (int i = 0; i < entries.Count; i++)
                    {
                        entries[i].UISelected = entries[i].Identifier == this.SelectedIdentifier;
                    }
                }
            });
        }
    }
}
