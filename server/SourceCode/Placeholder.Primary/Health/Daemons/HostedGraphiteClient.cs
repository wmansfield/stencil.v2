using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Placeholder.Primary.Health.Daemons
{
    public class HostedGraphiteClient : IDisposable
    {
        public HostedGraphiteClient(string api_key, string hostname, int port = 2003)
        {
            this.ApiKey = api_key;
            this.TcpClient = new TcpClient(hostname, port);
        }
        public TcpClient TcpClient { get; private set; }
        public string ApiKey { get; set; }

        public void SendMany(List<string> rawValues, bool allowRetry)
        {
            try
            {
                if(rawValues == null || rawValues.Count == 0)
                {
                    return;
                }
                StringBuilder builder = new StringBuilder();
                foreach (var item in rawValues)
                {
                    builder.Append(string.Format("{0}.{1}\n", this.ApiKey, item));
                }

                byte[] message = Encoding.UTF8.GetBytes(builder.ToString());
                this.TcpClient.GetStream().Write(message, 0, message.Length);
            }
            catch
            {
                // Supress all exceptions for now.
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (this.TcpClient != null)
            {
                this.TcpClient.Close();
            }
        }
    }
}
