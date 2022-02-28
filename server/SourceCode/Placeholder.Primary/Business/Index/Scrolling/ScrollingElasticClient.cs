using System;
using Elasticsearch.Net;
using Nest;

namespace Placeholder.Primary.Business.Index.Scrolling
{
    public class ScrollingElasticClient : ElasticClient, IElasticClient
    {
        public ScrollingElasticClient()
            : base()
        {

        }
        public ScrollingElasticClient(IConnectionSettingsValues connectionSettings)
            : base(connectionSettings)
        {

        }
        public ScrollingElasticClient(Uri uri)
            : base(uri)
        {

        }
        public ScrollingElasticClient(ITransport<IConnectionSettingsValues> transport)
            : base(transport)
        {

        }


        public int? ScrollThreshold { get; set; } = 500;
        public string ScrollWindow { get; set; } = "15s";

        
        ISearchResponse<T> IElasticClient.Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector)
        {
            return _ScrollingExtensions.SearchWithScroll(this, this.ScrollThreshold.GetValueOrDefault(), this.ScrollWindow, selector);
        }

        public ISearchResponse<T> SearchWithoutScroll<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
            where T : class
        {
            return base.Search<T>(selector);
        }

        public ISearchResponse<T> SearchWithScroll<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            return _ScrollingExtensions.SearchWithScroll(this, this.ScrollThreshold.GetValueOrDefault(), this.ScrollWindow, selector);
        }

    }
}
