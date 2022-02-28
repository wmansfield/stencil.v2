using System;

namespace Placeholder.SDK
{
    public class ItemResult<T> : ActionResult
    {
        public ItemResult()
        {
        }
        public ItemResult(T item, string meta = null)
        {
            this.item = item;
            this.meta = meta;
        }
        public T item { get; set; }
        public string meta { get; set; }
    }
    public class ItemResult<TData, TMeta> : ActionResult
    {
        public ItemResult()
        {
            this.meta = default(TMeta);
        }

        public ItemResult(TData item, TMeta meta)
        {
            this.item = item;
            this.meta = meta;
        }

        public TData item { get; set; }
        public virtual TMeta meta { get; set; }
    }
}
