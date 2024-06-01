using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Helper
{
    public static class HttpClientFactory 
    {
        private static readonly string _baseApiUrl = "https://localhost:7263"; // Replace with your actual URL

        public static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseApiUrl);
            return client;
        }
    }

}
