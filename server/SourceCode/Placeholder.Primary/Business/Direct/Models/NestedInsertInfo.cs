using System;
using System.Collections.Generic;
using Placeholder.Domain;

namespace Placeholder.Primary.Business.Direct
{
    public class NestedInsertInfo<TDbModel, TDomainModel>
        where TDomainModel : DomainModel
    {
        public NestedInsertInfo()
        {
            this.Chain = new HashSet<string>();
        }
        public TDbModel DbModel { get; set; }
        public TDomainModel InsertModel { get; set; }
        public HashSet<string> Chain { get; set; }
    }
}
