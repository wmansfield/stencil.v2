using Placeholder.Common;
using Placeholder.Primary.Health;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zero.Foundation;
using Zero.Foundation.Aspect;
using Zero.Foundation.System;
using Zero.Foundation.Unity;

namespace Placeholder.Primary.Business.Index
{
    public abstract class DualIndexerBase<TModel> : IndexerBaseHealth, IIndexer<TModel>
         where TModel : class
    {
        public DualIndexerBase(IFoundation iFoundation, string trackPrefix, string documentType)
           : base(iFoundation, trackPrefix)
        {
            this.DocumentType = documentType;
            this.API = new PlaceholderAPI(iFoundation);
            this.SharedCacheStatic5 = new AspectCache("DualIndexerBase5" + trackPrefix, iFoundation, new ExpireStaticLifetimeManager("DualIndexerBase5" + trackPrefix, TimeSpan.FromMinutes(5)));
            this.SharedCacheStatic15 = new AspectCache("DualIndexerBase" + trackPrefix, iFoundation, new ExpireStaticLifetimeManager("DualIndexerBase" + trackPrefix, TimeSpan.FromMinutes(15)));
        }

        #region Protected Properties

        public AspectCache SharedCacheStatic15 { get; set; }
        public AspectCache SharedCacheStatic5 { get; set; }

        public virtual PlaceholderAPI API { get; set; }

        public virtual string DocumentType { get; protected set; }

        public virtual IPlaceholderElasticClientFactory ClientFactory
        {
            get
            {
                return this.IFoundation.Resolve<IPlaceholderElasticClientFactory>();
            }
        }

        #endregion


        #region Abstract Methods

        protected abstract string GetModelId(TModel model);

        #endregion

        #region Public Methods

        public virtual Task<IndexResult> CreateAsync(TModel model)
        {
            return this.UpdateAsync(model);
        }

        public virtual Task<IndexResult> DeleteAsync(TModel model)
        {
            return base.ExecuteFunctionWriteAsync("DeleteAsync", async delegate ()
            {
                List<IElasticClient> clients = this.ClientFactory.CreateWriteClients();

                Exception allException = null;
                string allError = "invalid";
                bool allSuccess = true;
                string version = string.Empty;


                foreach (IElasticClient client in clients)
                {
                    Exception localException = null;
                    string localError = string.Empty;
                    bool localSuccess = false;

                    // CAREFUL, also used in UPDATE when intercepted
                    DeleteRequest request = new DeleteRequest(client.ConnectionSettings.DefaultIndex, this.DocumentType, GetModelId(model).ToString());
                    IDeleteResponse response = await client.DeleteAsync(request);
                    if (!response.IsValid)
                    {
                        localSuccess = false;
                        localError = "invalid";

                        if (response.ServerError != null && response.ServerError.Error != null && !string.IsNullOrEmpty(response.ServerError.Error.Reason))
                        {
                            localError = response.ServerError.Error.Reason;
                        }
                    }
                    else
                    {
                        localSuccess = true;
                        version = response.Version;
                    }


                    if (!string.IsNullOrEmpty(localError))
                    {
                        allError += localError;
                    }
                    if (localException != null)
                    {
                        allException = localException;
                    }
                    if (!localSuccess)
                    {
                        allSuccess = false;
                    }
                }

                if (allException != null)
                {
                    throw allException;
                }
                return new IndexResult()
                {
                    success = allSuccess,
                    error = allError,
                    version = version.ToString()
                };
            });
        }

        public virtual Task<IndexResult> UpdateAsync(TModel model)
        {
            return base.ExecuteFunctionWriteAsync("UpdateAsync", async delegate ()
            {
                try
                {
                    Exception allException = null;
                    string allError = "invalid";
                    bool allSuccess = true;
                    long version = 0;

                    List<IElasticClient> clients = this.ClientFactory.CreateWriteClients();

                    foreach (IElasticClient client in clients)
                    {
                        Exception localException = null;
                        string localError = string.Empty;
                        bool localSuccess = false;

                        // try twice per index
                        for (int i = 0; i < 2; i++)
                        {
                            try
                            {
                                localException = null;
                                IndexRequest<TModel> request = new IndexRequest<TModel>(client.ConnectionSettings.DefaultIndex, this.DocumentType, this.GetModelId(model).ToString());
                                request.Document = model;

                                IIndexResponse result = await client.IndexAsync(request);
                                if (!result.IsValid)
                                {
                                    HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_INSTANT_FAIL_SOFT_FORMAT, typeof(TModel).FriendlyName()), 0, 1);

                                    if (result.OriginalException != null && result.OriginalException.Message.Contains("400"))
                                    {
                                        this.API.Integration.Email.SendAdminEmail("Elastic Search 400 Error", string.Format("Error: {1}<br/><br/>Received from object: <br/>{0}", model, result.OriginalException.Message));
                                    }
                                    if (result.ServerError != null && result.ServerError.Error != null && !string.IsNullOrEmpty(result.ServerError.Error.Reason))
                                    {
                                        localError = result.ServerError.Error.Reason;
                                    }
                                    // allow it to try again
                                }
                                else
                                {
                                    localSuccess = true;
                                    version = result.Version;
                                    break;
                                }
                            }
                            catch (Elasticsearch.Net.ElasticsearchClientException cex)
                            {
                                localException = new Exception(cex.DebugInformation, cex);
                            }
                            catch (Exception ex)
                            {
                                localException = ex;
                            }
                        }
                        if (!string.IsNullOrEmpty(localError))
                        {
                            allError += localError;
                        }
                        if (localException != null)
                        {
                            allException = localException;
                        }
                        if (!localSuccess)
                        {
                            allSuccess = false;
                        }
                    }

                    if (allException != null)
                    {
                        throw allException;
                    }
                    return new IndexResult()
                    {
                        success = allSuccess,
                        error = allError,
                        version = version.ToString()
                    };
                }
                catch (Exception ex)
                {
                    this.IFoundation.LogError(ex, "UpdateDocument");
                    HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_INSTANT_FAIL_TIMEOUT_FORMAT, typeof(TModel).FriendlyName()), 0, 1);
                    return new IndexResult()
                    {
                        success = false,
                        error = ex.Message
                    };
                }
            });
        }

        public virtual Task<TModel> RetrieveByIdAsync(Guid id)
        {
            return base.ExecuteFunctionAsync("RetrieveByIdAsync", async delegate ()
            {
                IElasticClient client = this.ClientFactory.CreateReadClient();

                GetRequest<TModel> request = new GetRequest<TModel>(client.ConnectionSettings.DefaultIndex, this.DocumentType, id);

                IGetResponse<TModel> result = await client.GetAsync<TModel>(request);

                return result.Source;
            });
        }

        public virtual Task<TCustomModel> RetrieveByIdAsync<TCustomModel>(Guid id)
            where TCustomModel : class
        {
            return base.ExecuteFunctionAsync("RetrieveByIdAsync", async delegate ()
            {
                IElasticClient client = this.ClientFactory.CreateReadClient();

                GetRequest<TCustomModel> request = new GetRequest<TCustomModel>(client.ConnectionSettings.DefaultIndex, this.DocumentType, id);

                IGetResponse<TCustomModel> result = await client.GetAsync<TCustomModel>(request);

                return result.Source;
            });
        }

        #endregion


    }
}
