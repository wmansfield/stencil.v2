using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Placeholder.Primary.Business.Store
{
    public class FilteredCosmosSerializer : CosmosSerializer
    {
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        public FilteredCosmosSerializer(CosmosSerializationOptions cosmosSerializerOptions)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = cosmosSerializerOptions.IgnoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include,
                Formatting = cosmosSerializerOptions.Indented ? Formatting.Indented : Formatting.None,
                ContractResolver = cosmosSerializerOptions.PropertyNamingPolicy == CosmosPropertyNamingPolicy.CamelCase
                    ? new CamelCasePropertyNamesContractResolver()
                    : null
            };

            _serializerSettings = jsonSerializerSettings;
        }

        private readonly JsonSerializerSettings _serializerSettings;
        private ConcurrentDictionary<Type, JsonSerializerSettings> _resolvers = new ConcurrentDictionary<Type, JsonSerializerSettings>();

        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)stream;
                }

                using (StreamReader sr = new StreamReader(stream))
                {
                    using (JsonTextReader jsonTextReader = new JsonTextReader(sr))
                    {
                        JsonSerializer jsonSerializer = this.GetSerializer<T>();
                        return jsonSerializer.Deserialize<T>(jsonTextReader);
                    }
                }
            }
        }

        public override Stream ToStream<T>(T input)
        {
            MemoryStream streamPayload = new MemoryStream();
            using (StreamWriter streamWriter = new StreamWriter(streamPayload, encoding: FilteredCosmosSerializer.DefaultEncoding, bufferSize: 1024, leaveOpen: true))
            {
                using (JsonWriter writer = new JsonTextWriter(streamWriter))
                {
                    writer.Formatting = Newtonsoft.Json.Formatting.None;
                    JsonSerializer jsonSerializer = this.GetSerializer<T>();
                    jsonSerializer.Serialize(writer, input);
                    writer.Flush();
                    streamWriter.Flush();
                }
            }

            streamPayload.Position = 0;
            return streamPayload;
        }

        public void EnsureSerializer<TEntity>(Func<FilteredSerializerContractResolver<TEntity>> resolver)
        {
            Type type = typeof(TEntity);
            if(!_resolvers.ContainsKey(type))
            {
                _resolvers[type] = new JsonSerializerSettings()
                {
                    NullValueHandling = _serializerSettings.NullValueHandling,
                    Formatting = _serializerSettings.Formatting,
                    ContractResolver = resolver()
                };
                _resolvers[typeof(TEntity[])] = new JsonSerializerSettings()
                {
                    NullValueHandling = _serializerSettings.NullValueHandling,
                    Formatting = _serializerSettings.Formatting,
                    ContractResolver = resolver()
                };
            }
        }

        private JsonSerializer GetSerializer<T>()
        {
            JsonSerializerSettings settings = null;

            if(!_resolvers.TryGetValue(typeof(T), out settings))
            {
                settings = _serializerSettings;
            }
            return JsonSerializer.Create(settings);
        }
    }

}
