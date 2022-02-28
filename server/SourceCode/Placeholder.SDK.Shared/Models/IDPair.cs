using System;

namespace Placeholder.SDK.Models
{
    public class IDPair
    {
        public string id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }

        public override string ToString()
        {
            return this.name;
        }
        public override bool Equals(object obj)
        {
            IDPair other = obj as IDPair;
            if (other != null)
            {
                return this.id == other.id;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
    }
}
