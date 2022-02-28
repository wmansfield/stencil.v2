using System;
using System.Collections.Generic;
using Nest;

namespace Placeholder.Primary.Business.Index.Scrolling
{
    public class ScrollingHitsMetaData<T> : HitsMetaData<T>
        where T : class
    {
        public ScrollingHitsMetaData(List<IHit<T>> sourceHits)
        {
            this.SourceHits = sourceHits;
        }
        public List<IHit<T>> SourceHits { get; set; }

        public new IReadOnlyCollection<IHit<T>> Hits
        {
            get
            {
                return this.SourceHits;
            }
        }
        public new long Total
        {
            get
            {
                return this.SourceHits.Count;
            }
        }
    }
}
