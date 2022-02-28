using Zero.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Placeholder.Primary.Business.Store;

namespace Placeholder.Primary
{
    public partial class PlaceholderAPIStore : BaseClass
    {
        public PlaceholderAPIStore(IFoundation ifoundation)
            : base(ifoundation)
        {
        }
        public IAccountStore Accounts
        {
            get { return this.IFoundation.Resolve<IAccountStore>(); }
        }
        public IShopStore Shops
        {
            get { return this.IFoundation.Resolve<IShopStore>(); }
        }
        public IShopIsolatedStore ShopIsolateds
        {
            get { return this.IFoundation.Resolve<IShopIsolatedStore>(); }
        }
        public IShopAccountStore ShopAccounts
        {
            get { return this.IFoundation.Resolve<IShopAccountStore>(); }
        }
        public IShopSettingStore ShopSettings
        {
            get { return this.IFoundation.Resolve<IShopSettingStore>(); }
        }
        public ICompanyStore Companies
        {
            get { return this.IFoundation.Resolve<ICompanyStore>(); }
        }
        
    }
}


