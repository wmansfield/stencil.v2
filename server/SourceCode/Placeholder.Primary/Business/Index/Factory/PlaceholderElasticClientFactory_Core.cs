using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Placeholder.SDK.Models;
using sdk = Placeholder.SDK.Models;

namespace Placeholder.Primary.Business.Index
{
    public partial class PlaceholderElasticClientFactory
    {
        partial void MapIndexModels(CreateIndexDescriptor indexer)
        {
            MappingsDescriptor descriptor = new MappingsDescriptor();
            
            indexer.Mappings(m => descriptor);
        }
    }
}
