using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Placeholder.Primary.Business.Store
{
    public class FilteredSerializerContractResolver<TEntity> : DefaultContractResolver
    {
        public FilteredSerializerContractResolver()
        {
            this.IgnoredProperties = new HashSet<string>();
            this.RenamedProperties = new Dictionary<string, string>();
        }

        public HashSet<string> IgnoredProperties { get; set; }
        public Dictionary<string, string> RenamedProperties { get; set; }


        public FilteredSerializerContractResolver<TEntity> IgnoreProperty(string propertyName)
        {
            this.IgnoredProperties.Add(propertyName);
            return this;
        }

        public FilteredSerializerContractResolver<TEntity> RenameProperty(string propertyName, string newJsonPropertyName)
        {
            this.RenamedProperties[propertyName] = newJsonPropertyName;
            return this;
        }
        
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (this.IgnoredProperties.Contains(property.PropertyName))
            {
                property.ShouldSerialize = delegate (object obj) { return false; };
                property.Ignored = true;
            }

            string renamedProperty = null;
            if (this.RenamedProperties.TryGetValue(property.PropertyName, out renamedProperty))
            {
                property.PropertyName = renamedProperty;
            }

            return property;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> result = base.CreateProperties(type, memberSerialization);
            foreach (KeyValuePair<string, string> item in this.RenamedProperties)
            {
                JsonProperty match = result.FirstOrDefault(x => x.PropertyName == item.Value);
                if (match != null)
                {
                    result.Add(new JsonProperty()
                    {
                        PropertyName = item.Key,
                        DeclaringType = match.DeclaringType,
                        PropertyType = match.PropertyType,
                        Writable = match.Writable,
                        Readable = match.Readable,
                        AttributeProvider = match.AttributeProvider,
                        ValueProvider = match.ValueProvider
                    });
                }
            }
            return result;
        }
    }
}
