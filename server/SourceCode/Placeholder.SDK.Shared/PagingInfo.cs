using System;

namespace Placeholder.SDK
{
    public class PagingInfo
    {
        public PagingInfo()
        {
        }
        public PagingInfo(int skip, long take, long total)
        {
            this.total_pages = 0;
            this.current_page = 1;

            this.total_items = total;
            this.page_size = take;

            if (total > 0 && take > 0)
            {
                this.total_pages = (int)Math.Ceiling(total / (double)take);
            }
            if ((skip + take) > take)
            {
                current_page = (int)Math.Ceiling((skip + take) / (double)take);
            }
        }

        public long total_items { get; set; }
        public long total_pages { get; set; }
        public long page_size { get; set; }
        public long current_page { get; set; }

    }
}
