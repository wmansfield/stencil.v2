using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Placeholder.Common;
using Placeholder.Primary.Health;
using Placeholder.SDK;
using Placeholder.SDK.Client;
using Zero.Foundation;

namespace Placeholder.Primary.Business.Store
{
    public static class _StoreExtensions
    {
        public static bool IsSuccess(this HttpStatusCode status)
        {
            return status >= HttpStatusCode.OK && status < HttpStatusCode.MultipleChoices;
        }
        public static bool IsSuccessOrMissing(this HttpStatusCode status)
        {
            return status.IsSuccess() || status == HttpStatusCode.NotFound;
        }

        public static async Task<T> FetchSingleAsync<T>(this IQueryable<T> queryable)
        {
            if (queryable == null)
            {
                return default(T);
            }

            FeedResponse<T> result = await queryable
                                        .Take(1)
                                        .ToFeedIterator()
                                        .ReadNextAsync();

            TrackRequestCharge(result);

            return result.FirstOrDefault();
        }

        public static async Task<List<T>> FetchAsListAsync<T>(this IQueryable<T> queryable, int skip = 0, int? take = null)
        {
            List<T> result = new List<T>();
            if (queryable != null)
            {
                int skipped = 0;
                using (FeedIterator<T> iterator = queryable.ToFeedIterator())
                {
                    while (iterator.HasMoreResults && (take == null || result.Count < take))
                    {
                        FeedResponse<T> response = await iterator.ReadNextAsync();

                        TrackRequestCharge(response);

                        foreach (T item in response)
                        {
                            if (skip > skipped)
                            {
                                skipped++;
                            }
                            else
                            {
                                if (take == null || result.Count < take)
                                {
                                    result.Add(item);
                                }
                                if (result.Count >= take)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static async Task<ListResult<T>> FetchAsSteppedListAsync<T>(this IQueryable<T> queryable, int skip, int take, bool includeTotal = true)
        {
            if (queryable == null)
            {
                return new ListResult<T>();
            }

            int takePlus = take;
            if (take != int.MaxValue)
            {
                takePlus++; // for stepping
            }

            List<T> data = await queryable.FetchAsListAsync(skip, takePlus);
            int total = 0;

            if(includeTotal)
            {
                total = await queryable.CountAsync();
            }
            return data.ToSteppedListResult(skip, take, total);
        }

        public static void TrackRequestCharge<T>(this Response<T> response)
        {
            try
            {
                if (response == null)
                {
                    return;
                }

                if (response.Headers != null)
                {
                    string methodName = CommonUtility.GetLatestMethodName();
                    if (string.IsNullOrEmpty(methodName))
                    {
                        methodName = System.IO.Path.GetExtension(typeof(T).ToString()).Trim('.').Trim(']') + ".Unknown";
                    }
                    string trackName = string.Format(HealthReporter.COSMOS_RU_FORMAT, methodName);
                    HealthReporter.Current.UpdateMetric(HealthTrackType.CountAndDurationAverage, trackName, (int)response.Headers.RequestCharge, 1);
                }
            }
            catch
            {
                // gulp
            }
        }

        private static IFoundation _iFoundation;
        /// <summary>
        /// A little dirty, but we don't care.
        /// </summary>
        private static IFoundation IFoundation
        {
            get
            {
                if (_iFoundation == null)
                {
                    _iFoundation = CoreFoundation.Current;
                }
                return _iFoundation;
            }
        }
    }
}
