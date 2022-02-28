using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Business.Store.Models
{
    public class TenantCreationData
    {
        public TenantCreationData()
        {
            this.ContainersEnsured = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            this.DatabasesEnsured = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        }
        public ConcurrentDictionary<string, bool> ContainersEnsured { get; set; }
        public ConcurrentDictionary<string, bool> DatabasesEnsured { get; set; }
    }
}
