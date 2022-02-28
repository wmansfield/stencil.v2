using System.Collections.Generic;
using Placeholder.Domain;

namespace Placeholder.Primary.Business.Direct
{
    public class NestedUpdateInfo<TDbModel, TDomainModel>
        where TDomainModel : DomainModel
    {
        public NestedUpdateInfo()
        {
            this.Chain = new HashSet<string>();
        }
        public TDbModel DbModel { get; set; }
        public TDomainModel UpdateModel { get; set; }
        public TDomainModel PreviousModel { get; set; }
        public HashSet<string> Chain { get; set; }

    }
}
