using System;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    public class InterceptArgs<T>
    {
        public bool Intercepted { get; set; }
        public T ReturnEntity { get; set; }
        public Crud Crud { get; internal set; }
        public Availability availability { get; set; }
    }
}
