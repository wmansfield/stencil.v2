using System;
using System.Diagnostics;

namespace Placeholder.Primary.Health
{
    public class HealthTrackScope : IDisposable
    {
        public HealthTrackScope(HealthTrackType trackType, string trackName, int count = 1)
        {
            this.Count = count;
            this.TrackType = trackType;
            this.TrackName = trackName;
            this.Stopwatch = Stopwatch.StartNew();
        }
        ~HealthTrackScope()
        {
            this.Dispose(false);
        }

        protected virtual HealthTrackType TrackType { get; set; }
        protected virtual string TrackName { get; set; }
        protected virtual Stopwatch Stopwatch { get; set; }
        protected virtual int Count { get; set; }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Stopwatch.Stop();
                if (this.TrackType != HealthTrackType.None)
                {
                    HealthReporter.Current.UpdateMetric(this.TrackType, this.TrackName, this.Stopwatch.ElapsedMilliseconds, this.Count);
                }
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
