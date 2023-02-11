using Starter.App;
using Starter.App.Presentation.Shells.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter
{
    /// <summary>
    /// Starter Pages happen before app fully loads, use caution
    /// </summary>
    public partial class LoadingPageLight : BlankPageLight
    {
        public LoadingPageLight()
        {
            InitializeComponent();
            lblMessage.Text = StarterAPI.Instance.Localize(I18NToken.ScreenLoading_LoadingAccount, "loading..");
        }
    }
}