using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Health.Daemons
{
    public class GrafanaGraphiteClient
    {
        public GrafanaGraphiteClient(string api_key, string api_url)
        {
            this.ApiUrl = api_url;
            this.ApiKey = api_key;
        }

        private static readonly HttpClient HttpClient = new HttpClient();

        public static string PAYLOAD_FORMAT = "";
        public static string INTERVAL = "10";

        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }

        public static string GenerateMetric(string metric, string interval, string value, string time)
        {
            return string.Format("{{\"name\": \"{0}\",\"interval\": {1},\"value\": {2},\"time\": {3} }}", metric, interval, value, time);
        }
        public static string GenerateGrafanaMetric(string graphite_format_log)
        {
            string[] data = graphite_format_log.Split(' ');
            if(data.Length == 3)
            {
                return GenerateMetric(data[0], INTERVAL, data[1], data[2]);
            }
            return null;
        }

        public void SendMany(List<string> graphite_format_logs, bool allowRetry)
        {
            try
            {
                List<string> metrics = new List<string>(graphite_format_logs.Count);
                foreach (var item in graphite_format_logs)
                {
                    string metric = GenerateGrafanaMetric(item);
                    if(metric != null)
                    {
                        metrics.Add(metric);
                    }
                }
                string metricPayload = "[" + string.Join(",", metrics) + "]";

                
                // Verify this works for your grafana service, some nuance there.
                HttpContent content = new StringContent(metricPayload, Encoding.UTF8);

                HttpRequestMessage request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(this.ApiUrl),
                    Method = HttpMethod.Post,
                    Content = content
                };
                request.Headers.Add("Authorization", string.Format("Bearer {0}", this.ApiKey));
                request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage responseMessage = HttpClient.SendAsync(request).Result;

                /*
                WebClient webClient = new WebClient();
                webClient.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", this.ApiKey));
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                string response = webClient.UploadString(this.ApiUrl, metricPayload);
                */
            }
            catch
            {
                if (allowRetry)
                {
                    Task.Run(async delegate ()
                    {
                        await Task.Delay(15000); // retry once, in 15 secs
                        this.SendMany(graphite_format_logs, false);
                    });
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
