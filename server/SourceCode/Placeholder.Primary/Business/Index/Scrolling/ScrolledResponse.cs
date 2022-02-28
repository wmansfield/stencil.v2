using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace Placeholder.Primary.Business.Index.Scrolling
{
    public class ScrolledResponse<T> : ISearchResponse<T>
       where T : class
    {
        public ScrolledResponse()
        {
            this.RawDocuments = new List<T>();
            this.RawHits = new List<IHit<T>>();
            this.HitsMetaData = new ScrollingHitsMetaData<T>(this.RawHits);
        }

        public int FillScroll(ISearchResponse<T> response, int skip, int? maxDocuments = null)
        {
            // Pass Thru
            this.Shards = response.Shards;
            this.ScrollId = response.ScrollId;
            this.TerminatedEarly = response.TerminatedEarly;
            this.TimedOut = response.TimedOut;
            this.Profile = response.Profile;
            this.ApiCall = response.ApiCall;
            this.DebugInformation = response.DebugInformation;
            this.IsValid = response.IsValid;
            this.ServerError = response.ServerError;
            this.OriginalException = response.OriginalException;
            this.TimedOut = response.TimedOut;

            // Collecting
            int countBeforeFill = this.RawDocuments.Count;

            this.RawHits.AddRange(response.Hits.Skip(skip));

            List<T> newData = response.Documents.Skip(skip).ToList();
            foreach (T item in newData)
            {
                if (!maxDocuments.HasValue || this.RawDocuments.Count < maxDocuments.Value)
                {
                    this.RawDocuments.Add(item);
                }
            }

            this.MaxScore = Math.Max(this.MaxScore, response.MaxScore);
            this.Took += response.Took;
            this.NumberOfReducePhases = response.NumberOfReducePhases;

            // First Only
            //this.Total += response.Total;
            //this.Aggs = response.Aggs;
            //this.Aggregations = Aggs.Aggregations;

            // Ignore
            //this.HitsMetaData = response.HitsMetaData;
            //this.Suggest = response.Suggest;
            //this.Fields = response.Fields;
            //this.Highlights = response.Highlights;

            return newData.Count;
        }

        public int FillInitial(ISearchResponse<T> response, int skip, int? maxDocuments = null)
        {
            // Pass Thru
            this.Shards = response.Shards;
            this.ScrollId = response.ScrollId;
            this.TerminatedEarly = response.TerminatedEarly;
            this.TimedOut = response.TimedOut;
            this.Profile = response.Profile;
            this.ApiCall = response.ApiCall;
            this.DebugInformation = response.DebugInformation;
            this.IsValid = response.IsValid;
            this.ServerError = response.ServerError;
            this.OriginalException = response.OriginalException;
            this.TimedOut = response.TimedOut;

            // Collecting
            this.RawHits.AddRange(response.Hits.Skip(skip));

            List<T> newData = response.Documents.Skip(skip).ToList();
            foreach (T item in newData)
            {
                if (!maxDocuments.HasValue || this.RawDocuments.Count < maxDocuments.Value)
                {
                    this.RawDocuments.Add(item);
                }
            }
            this.MaxScore = Math.Max(this.MaxScore, response.MaxScore);
            this.Took = response.Took;
            this.NumberOfReducePhases = response.NumberOfReducePhases;

            // First Only
            this.Aggs = response.Aggs;
            this.Aggregations = Aggs.Aggregations;
            this.Total = response.Total;

            // Ignore
            //this.HitsMetaData = response.HitsMetaData;
            //this.Suggest = response.Suggest;
            //this.Fields = response.Fields;

            //this.Highlights = response.Highlights;

            return newData.Count;
        }

        public List<T> RawDocuments { get; set; }
        public IReadOnlyCollection<T> Documents
        {
            get
            {
                return this.RawDocuments;
            }
        }


        public ScrollingHitsMetaData<T> HitsMetaData { get; set; }

        public ShardsMetaData Shards { get; set; }

        HitsMetaData<T> ISearchResponse<T>.HitsMetaData
        {
            get
            {
                return this.HitsMetaData;
            }
        }

        public AggregationsHelper Aggs { get; set; }
        public IReadOnlyDictionary<string, IAggregate> Aggregations { get; set; }
        public IReadOnlyDictionary<string, Suggest<T>[]> Suggest
        {
            get
            {
                throw new Exception("Not supported when automated scrolling is enabled");
            }
        }
        public IReadOnlyCollection<FieldValues> Fields
        {
            get
            {
                throw new Exception("Not supported when automated scrolling is enabled");
            }
        }
        
        public List<IHit<T>> RawHits { get; set; }
        public IReadOnlyCollection<IHit<T>> Hits
        {
            get
            {
                return this.RawHits;
            }
        }

        public long NumberOfReducePhases { get; set; }

        public Profile Profile { get; set; }

        public long Took { get; set; }

        public bool TimedOut { get; set; }

        public bool TerminatedEarly { get; set; }

        public string ScrollId { get; set; }

        public long Total { get; set; }

        public double MaxScore { get; set; }

        public bool IsValid { get; set; }

        public IApiCallDetails ApiCall { get; set; }

        public ServerError ServerError { get; set; }

        public Exception OriginalException { get; set; }

        public string DebugInformation { get; set; }

        public IApiCallDetails CallDetails { get; set; }

    }
}
