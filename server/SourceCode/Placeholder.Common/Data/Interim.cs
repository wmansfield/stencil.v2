using System;

namespace Placeholder.Common.Data
{
    public class Interim<T1, T2, T3, T4, T5, T6> : Interim<T1, T2, T3, T4, T5>
    {
        public T6 Item6 { get; set; }
    }
    public class Interim<T1, T2, T3, T4, T5> : Interim<T1, T2, T3, T4>
    {
        public T5 Item5 { get; set; }
    }
    public class Interim<T1, T2, T3, T4> : Interim<T1, T2, T3>
    {
        public T4 Item4 { get; set; }
    }
    public class Interim<T1, T2, T3> : Interim<T1, T2>
    {
        public T3 Item3 { get; set; }
    }
    public class Interim<T1, T2> : Interim<T1>
    {
        public T2 Item2 { get; set; }
    }
    public class Interim<T1>
    {
        public T1 Item1 { get; set; }
    }
}
