using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Business.Index
{
    public interface IIndexer<TModel>
        where TModel : class
    {
        Task<IndexResult> CreateAsync(TModel model);
        Task<IndexResult> DeleteAsync(TModel model);
        Task<IndexResult> UpdateAsync(TModel model);
        Task<TCustomModel> RetrieveByIdAsync<TCustomModel>(Guid id)
            where TCustomModel : class;
    }
}
