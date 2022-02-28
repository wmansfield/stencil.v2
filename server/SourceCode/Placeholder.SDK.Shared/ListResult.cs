using System;
using System.Collections.Generic;
using System.Linq;

namespace Placeholder.SDK
{
    public class ListResult<T> : ActionResult
    {
        public ListResult()
        {
            this.items = new List<T>();
        }
        public ListResult(List<T> items)
        {
            this.items = items;
        }
        public ListResult(List<T> items, PagingInfo paging)
        {
            this.items = items;
            this.paging = paging;
        }
        public ListResult(List<T> items, SteppingInfo stepping)
        {
            this.items = items;
            this.stepping = stepping;
        }

        public ListResult(IEnumerable<T> items, PagingInfo paging)
        {
            this.items = items.ToList();
            this.paging = paging;
        }
        public ListResult(IEnumerable<T> items, SteppingInfo stepping)
        {
            this.items = items.ToList();
            this.stepping = stepping;
        }

        public virtual List<T> items { get; set; }

        public virtual PagingInfo paging { get; set; }
        public virtual SteppingInfo stepping { get; set; }
    }

    public class ListResult<TData, TMeta> : ListResult<TData>
        where TMeta : new()
    {
        public ListResult()
            : base()
        {
            this.meta = new TMeta();
        }

        public virtual TMeta meta { get; set; }
    }
}
