using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Nest;
using Placeholder.Common;
using Placeholder.Primary.Business.Index.Scrolling;
using Zero.Foundation;
using Zero.Foundation.Aspect;

namespace Placeholder.Primary.Business.Index
{
    public partial class PlaceholderElasticClientFactory : ChokeableClass, IPlaceholderElasticClientFactory
    {
        public PlaceholderElasticClientFactory(IFoundation foundation)
            : base(foundation)
        {
            this.API = this.IFoundation.Resolve<PlaceholderAPI>();
            _ensuredModelIndices = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        }

        private ConnectionSettings _primaryConnectionSettings;
        private ConnectionSettings _secondaryConnectionSettings;

        protected PlaceholderAPI API { get; set; }
        protected virtual ConnectionSettings PrimaryConnectionSettings
        {
            get
            {
                if (_primaryConnectionSettings == null)
                {
                    bool isLocalHost = this.API.Integration.Settings.IsLocalHost();

                    string hostUrl = this.API.Integration.Settings.GetSetting(CommonAssumptions.APP_KEY_ES_URL);
                    _primaryConnectionSettings = new ConnectionSettings(new Uri(hostUrl))
                        .DefaultIndex(this.IndexName);

                    if (isLocalHost || this.QueryDebugEnabled)
                    {
                        _primaryConnectionSettings.DisableDirectStreaming();
                        _primaryConnectionSettings.OnRequestCompleted(details =>
                        {
                            this.IFoundation.LogTrace(" --- ElasticSearch REQEUST --- ");
                            if (details.RequestBodyInBytes != null)
                            {
                                System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                            }
                            this.IFoundation.LogTrace(" --- ElasticSearch RESPONSE --- ");
                            if (details.ResponseBodyInBytes != null)
                            {
                                System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
                            }
                        });
                    }
                }
                return _primaryConnectionSettings;
            }
        }
        protected virtual ConnectionSettings SecondaryConnectionSettings
        {
            get
            {
                if (_secondaryConnectionSettings == null)
                {
                    bool isLocalHost = this.API.Integration.Settings.IsLocalHost();

                    string hostUrl = this.API.Integration.Settings.GetSetting(CommonAssumptions.APP_KEY_ES_SECONDARY_URL);
                    if (!string.IsNullOrEmpty(hostUrl))
                    {
                        _secondaryConnectionSettings = new ConnectionSettings(new Uri(hostUrl))
                            .DefaultIndex(this.IndexName);

                        if (isLocalHost || this.QueryDebugEnabled)
                        {
                            _secondaryConnectionSettings.DisableDirectStreaming();
                            _secondaryConnectionSettings.OnRequestCompleted(details =>
                            {
                                this.IFoundation.LogTrace(" --- ElasticSearch REQEUST --- ");
                                if (details.RequestBodyInBytes != null)
                                {
                                    this.IFoundation.LogTrace(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                                }
                                this.IFoundation.LogTrace(" --- ElasticSearch RESPONSE --- ");
                                if (details.ResponseBodyInBytes != null)
                                {
                                    this.IFoundation.LogTrace(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
                                }
                            });
                        }
                    }
                }
                return _secondaryConnectionSettings;
            }
        }
        public virtual string IndexName
        {
            get
            {
                return this.API.Integration.Settings.GetSetting(CommonAssumptions.APP_KEY_ES_INDEX);
            }
        }

        public virtual int ReplicaCount
        {
            get
            {
                return int.Parse(this.API.Integration.Settings.GetSetting(CommonAssumptions.APP_KEY_ES_REPLICA));
            }
        }
        public virtual int ShardCount
        {
            get
            {
                return int.Parse(this.API.Integration.Settings.GetSetting(CommonAssumptions.APP_KEY_ES_SHARDS));
            }
        }
        public virtual bool QueryDebugEnabled
        {
            get
            {
                bool result = false;
                bool.TryParse(this.API.Integration.Settings.GetSetting(CommonAssumptions.APP_KEY_DEBUG_QUERIES), out result);
                return result;
            }
        }

        private ConcurrentDictionary<string, bool> _ensuredModelIndices;

        public virtual bool HasEnsuredModelIndices(string name)
        {
            bool hasEnsured = false;
            if (_ensuredModelIndices.TryGetValue(name, out hasEnsured) && hasEnsured)
            {
                return true;
            }
            return false;
        }
        public virtual void SetHasEnsuredModelIndices(string name, bool hasEnsured)
        {
            _ensuredModelIndices[name] = hasEnsured;
        }

        public virtual IElasticClient CreateReadClient()
        {
            return base.ExecuteFunction("CreateReadClient", delegate ()
            {
                IElasticClient result = new ScrollingElasticClient(this.PrimaryConnectionSettings);

                this.EnsureModelIndices(result, "primary");
                return result;
            });
        }
        public virtual List<IElasticClient> CreateWriteClients()
        {
            return base.ExecuteFunction("CreateWriteClients", delegate ()
            {
                List<IElasticClient> result = new List<IElasticClient>();

                ConnectionSettings primarySettings = this.PrimaryConnectionSettings;
                ConnectionSettings secondarySettings = this.SecondaryConnectionSettings;
                bool isHydrate = this.API.Integration.Settings.IsHydrate();

                // dont do primary if we are hydrating and we have a secondary
                if (!isHydrate || secondarySettings == null)
                {
                    IElasticClient primary = new ScrollingElasticClient(primarySettings);
                    this.EnsureModelIndices(primary, "primary");
                    result.Add(primary);
                }
                if (secondarySettings != null)
                {
                    IElasticClient secondary = new ScrollingElasticClient(secondarySettings);
                    this.EnsureModelIndices(secondary, "secondary");
                    result.Add(secondary);
                }

                return result;
            });
        }

        private static bool debug_reset = false;
        private static object debug_lock = new object();
        private static object ensure_lock = new object();

        protected void EnsureModelIndices(IElasticClient client, string name)
        {
            base.ExecuteMethod("EnsureModelIndices", delegate ()
            {
                if (debug_reset)
                {
                    bool executeDebug = false;
                    lock (debug_lock)
                    {
                        if (debug_reset)
                        {
                            executeDebug = true;
                        }
                        debug_reset = false;
                    }
                    if (executeDebug)
                    {
                        //client.Map<Objective>(m => m
                        //            .MapFromAttributes()
                        //            .Type(DocumentTypes.OBJECTIVES)
                        //            .Properties(props => props
                        //                .String(s => s
                        //                    .Name(p => p.campaign_id)
                        //                    .Index(FieldIndexOption.NotAnalyzed))
                        //                .String(s => s
                        //                    .Name(p => p.objective_id)
                        //                    .Index(FieldIndexOption.NotAnalyzed)
                        //            ))
                        //         );
                    }
                }
                if (!this.HasEnsuredModelIndices(name))
                {
                    lock (ensure_lock)
                    {
                        if (!this.HasEnsuredModelIndices(name))
                        {
                            if (!client.IndexExists(this.IndexName).Exists)
                            {
                                CustomAnalyzer ignoreCaseAnalyzer = new CustomAnalyzer
                                {
                                    Tokenizer = "keyword",
                                    Filter = new[] { "lowercase" }
                                };
                                Analysis analysis = new Analysis();
                                analysis.Analyzers = new Analyzers();
                                analysis.Analyzers.Add("case_insensitive", ignoreCaseAnalyzer);
                                ICreateIndexResponse createResult = client.CreateIndex(this.IndexName, delegate (Nest.CreateIndexDescriptor descriptor)
                                {
                                    descriptor.Settings(ss => ss
                                        .Analysis(a => analysis)
                                        .NumberOfReplicas(this.ReplicaCount)
                                        .NumberOfShards(this.ShardCount)
                                        .Setting("index.mapping.total_fields.limit", "2000")
                                        .Setting("search.slowlog.threshold.fetch.warn", "1s")
                                        .Setting("max_result_window", "2147483647")
                                    );
                                    this.MapIndexModels(descriptor);
                                    return descriptor;
                                });
                                if (!createResult.Acknowledged)
                                {
                                    throw new Exception("Error creating index, mapping is no longer valid");
                                }
                            }
                            this.SetHasEnsuredModelIndices(name, true);
                        }
                    }
                }
            });
        }

        partial void MapIndexModels(CreateIndexDescriptor indexer);
    }
}
