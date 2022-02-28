using System;

namespace Placeholder.Primary.Health
{
     public interface IHealthExtractor
    {
        void ExtractHealthMetrics(HealthReportGenerator generator);
    }
}
