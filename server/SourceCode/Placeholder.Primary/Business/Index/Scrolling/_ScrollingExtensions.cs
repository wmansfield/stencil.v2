using System;
using System.Linq;
using Nest;

namespace Placeholder.Primary.Business.Index.Scrolling
{
     public static class _ScrollingExtensions
    {
        public static ISearchResponse<T> SearchWithScroll<T>(this IElasticClient client, Func<SearchDescriptor<T>, ISearchRequest> selector)
            where T : class
        {
            return client.SearchWithScroll<T>(2000, "15s", selector);
        }
        public static ISearchResponse<T> SearchWithScroll<T>(this IElasticClient client, int scrollThreshold, string scrollWindowTime, Func<SearchDescriptor<T>, ISearchRequest> selector)
            where T : class
        {
            ISearchRequest request = selector(new SearchDescriptor<T>());
            if (request.Size.HasValue && scrollThreshold > 0 && request.Size.Value > scrollThreshold)
            {
                // Try to switch to scrolling API
                SearchDescriptor<T> interim = request as SearchDescriptor<T>;
                if (interim != null)
                {
                    int skip = request.From.GetValueOrDefault();
                    int targetSize = request.Size.Value;
                    request = interim.Size(scrollThreshold).Scroll(scrollWindowTime);

                    ISearchResponse<T> response = client.Search<T, T>(request);

                    ScrolledResponse<T> scrolledResponse = new ScrolledResponse<T>();
                    int nextskip = skip + response.Documents.Count();

                    int addedData = scrolledResponse.FillInitial(response, skip, targetSize);
                    skip = nextskip;

                    bool hasMore = addedData >= scrollThreshold;

                    while (scrolledResponse.RawDocuments.Count < targetSize && response.IsValid && hasMore && !string.IsNullOrEmpty(response.ScrollId))
                    {
                        response = client.Scroll<T>(scrollWindowTime, response.ScrollId);
                        nextskip += response.Documents.Count();

                        addedData = scrolledResponse.FillScroll(response, 0, targetSize);
                        hasMore = addedData >= scrollThreshold;
                    }

                    if (!string.IsNullOrEmpty(response.ScrollId))
                    {
                        try
                        {
                            client.ClearScroll(x => x.ScrollId(response.ScrollId));
                        }
                        catch
                        {
                            // gulp
                        }
                    }

                    return scrolledResponse;
                }
            }

            // fallthrough
            return client.Search<T, T>(request);

        }
    }
}
