using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Placeholder.SDK.Client.Exceptions;
using Placeholder.SDK.Client.Security;
using Placeholder.SDK.Client.Serialization;
using RestSharp;

namespace Placeholder.SDK.Client
{
    public partial class PlaceholderSDK
    {
        #region Constructor

        public PlaceholderSDK(string baseUrl)
            : this(string.Empty, string.Empty, baseUrl)
        {
        }
        public PlaceholderSDK(string applicationKey, string applicationSecret)
            : this(applicationKey, applicationSecret, API_BASE_URL)
        {
        }

        public PlaceholderSDK(PlaceholderSDKAuthInfo authInfo)
            : this(authInfo.ApiKey, authInfo.ApiSecret, API_BASE_URL)
        {

        }
        public PlaceholderSDK(PlaceholderSDKAuthInfo authInfo, string baseUrl)
            : this(authInfo.ApiKey, authInfo.ApiSecret, baseUrl)
        {

        }
        public PlaceholderSDK(string applicationKey, string applicationSecret, string baseUrl)
        {
            this.CustomHeaders = new List<KeyValuePair<string, string>>();
            this.SignatureGenerator = new HashedTimeSignatureGenerator();

            this.AsyncTimeoutMillisecond = (int)TimeSpan.FromSeconds(40).TotalMilliseconds;
            this.ApplicationKey = applicationKey;
            this.ApplicationSecret = applicationSecret;
            if (baseUrl == null)
            {
                baseUrl = API_BASE_URL;
            }
            this.BaseUrl = baseUrl;

            this.InstanceCache = new Dictionary<string, object>();

            this.ConstructCoreEndpoints();
            this.ConstructManualEndpoints();
        }

        #endregion

        #region Constants

        public const string API_BASE_URL = "https://placeholder.foundationzero.com/api";
        protected const string API_PARAM_KEY = "x-api-key";
        protected const string API_PARAM_SIG = "x-api-signature";

        #endregion

        #region Properties

        public int AsyncTimeoutMillisecond; // member for web ease
        public int WebRequestTimeoutMillisecond; // member for web ease

        public string BaseUrl; // member for web ease
        public string ApplicationKey; // member for web ease
        public string ApplicationSecret; // member for web ease
        /// <summary>
        /// adds the headers to every request
        /// </summary>
        public List<KeyValuePair<string, string>> CustomHeaders { get; set; }

        public HashedTimeSignatureGenerator SignatureGenerator { get; set; }
        protected internal Dictionary<string, object> InstanceCache { get; set; }

        /// <summary>
        /// reference only, not used
        /// </summary>
        public Guid ShopID { get; set; }

        #endregion

        #region Public Methods

        public IRestResponse Execute(RestRequest request)
        {
            RestClient client = new RestClient();
            

            this.PrepareRequest(client, request);

            IRestResponse response = client.Execute(request);

            this.ValidateResponse(response);

            return response;
        }
        public T Execute<T>(RestRequest request)
            where T : new()
        {
            RestClient client = new RestClient();

            this.PrepareRequest(client, request);

            IRestResponse<T> response = client.Execute<T>(request);

            this.ValidateResponse(response);

            return response.Data;
        }

        public Task<IRestResponse> ExecuteAsync(RestRequest request)
        {
            return this.ExecuteAsync(request, this.AsyncTimeoutMillisecond);
        }
        public Task<IRestResponse> ExecuteAsync(RestRequest request, int milliSecondTimeout)
        {
            if (milliSecondTimeout <= 0)
            {
                milliSecondTimeout = this.AsyncTimeoutMillisecond;
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(milliSecondTimeout);

            return this.ExecuteAsyncInternal(request, cancellationTokenSource.Token);
        }

        public Task<T> ExecuteAsync<T>(RestRequest request)
            where T : new()
        {
            return this.ExecuteAsync<T>(request, this.AsyncTimeoutMillisecond);
        }
        
        public Task<T> ExecuteAsync<T>(RestRequest request, int milliSecondTimeout)
            where T : new()
        {
            if (milliSecondTimeout <= 0)
            {
                milliSecondTimeout = this.AsyncTimeoutMillisecond;
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(milliSecondTimeout);

            return this.ExecuteAsyncInternal<T>(request, cancellationTokenSource.Token);
        }

        #endregion

        #region Protected Methods

        protected virtual void PrepareRequest(RestClient client, RestRequest request)
        {
            client.BaseUrl = new Uri(BaseUrl);

            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new NewtonSoftSerializer();

            if (this.WebRequestTimeoutMillisecond > 0)
            {
                client.Timeout = this.WebRequestTimeoutMillisecond;
            }

            this.AddAuthorizationHeaders(client, request);
            this.AddCustomHeaders(client, request);
        }
        
        protected virtual async Task<IRestResponse> ExecuteAsyncInternal(RestRequest request, CancellationToken token = default)
        {
            try
            {
                RestClient client = new RestClient();

                this.PrepareRequest(client, request);

                IRestResponse response = await client.ExecuteAsync(request, token);

                this.ValidateResponse(response, token);

                return response;
            }
            catch (ThreadAbortException)
            {
                throw new EndpointTimeoutException(System.Net.HttpStatusCode.BadGateway, "Error communicating with server, connection terminated.");
            }
            catch (OperationCanceledException)
            {
                throw new EndpointTimeoutException(System.Net.HttpStatusCode.GatewayTimeout, "Error communicating with server, connection timed out.");
            }
        }
        protected virtual async Task<T> ExecuteAsyncInternal<T>(RestRequest request, CancellationToken token = default)
            where T : new()
        {
            try
            {
                RestClient client = new RestClient();

                this.PrepareRequest(client, request);

                IRestResponse response = await client.ExecuteAsync(request, token);

                this.ValidateResponse(response, token);

                string content = response.Content;

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (ThreadAbortException)
            {
                throw new EndpointTimeoutException(System.Net.HttpStatusCode.BadGateway, "Error communicating with server, connection terminated.");
            }
            catch (OperationCanceledException)
            {
                throw new EndpointTimeoutException(System.Net.HttpStatusCode.GatewayTimeout, "Error communicating with server, connection timed out.");
            }
        }


        protected virtual void AddCustomHeaders(RestClient client, RestRequest request)
        {
            if (this.CustomHeaders != null)
            {
                foreach (var item in this.CustomHeaders)
                {
                    if (!string.IsNullOrEmpty(item.Key))
                    {
                        client.AddDefaultHeader(item.Key, item.Value);
                    }
                }
            }
        }
        protected virtual void AddAuthorizationHeaders(RestClient client, RestRequest request)
        {
            List<KeyValuePair<string, string>> authHeaders = this.GenerateAuthorizationHeader();
            foreach (var item in authHeaders)
            {
                client.AddDefaultHeader(item.Key, item.Value);
            }
        }
        
        public virtual List<KeyValuePair<string, string>> GenerateAuthorizationHeader()
        {
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(this.ApplicationKey) && !string.IsNullOrEmpty(this.ApplicationSecret))
            {
                headers.Add(new KeyValuePair<string, string>(API_PARAM_KEY, this.ApplicationKey));
                headers.Add(new KeyValuePair<string, string>(API_PARAM_SIG, this.SignatureGenerator.CreateSignature(this.ApplicationKey, this.ApplicationSecret)));
            }
            return headers;
        }
        protected virtual void ValidateResponse(IRestResponse response, CancellationToken token = default)
        {
            switch (response.StatusCode)
            {

                case System.Net.HttpStatusCode.Continue:
                case System.Net.HttpStatusCode.Accepted:
                case System.Net.HttpStatusCode.Created:
                case System.Net.HttpStatusCode.NoContent:
                case System.Net.HttpStatusCode.NotModified:
                case System.Net.HttpStatusCode.OK:
                    // do nothing
                    break;
                default:
                    throw new EndpointException(response.StatusCode, response.StatusDescription);
            }
            if (response.ErrorException != null)
            {
                throw new ApplicationException("Error retrieving response.  Check inner details for more info.", response.ErrorException);
            }
        }

        #endregion

    }
}
