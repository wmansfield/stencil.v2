using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Placeholder.Primary.Business.Store
{
    public class BulkOperations<T>
    {
        public BulkOperations(int operationCount)
        {
            this.Tasks = new List<Task<OperationResponse<T>>>(operationCount);
        }

        public readonly List<Task<OperationResponse<T>>> Tasks;

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public async Task<BulkOperationResponse<T>> ExecuteAsync()
        {
            await Task.WhenAll(this.Tasks);
            _stopwatch.Stop();
            return new BulkOperationResponse<T>()
            {
                TotalTimeTaken = _stopwatch.Elapsed,
                TotalRequestUnitsConsumed = this.Tasks.Sum(task => task.Result.RequestUnitsConsumed),
                SuccessfulDocuments = this.Tasks.Count(task => task.Result.IsSuccessful),
                Failures = this.Tasks.Where(task => !task.Result.IsSuccessful).Select(task => (task.Result.Item, task.Result.CosmosException)).ToList()
            };
        }
    }
}
