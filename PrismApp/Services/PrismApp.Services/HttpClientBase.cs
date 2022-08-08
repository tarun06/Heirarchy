using System.Net.Http;
using System.Net.Http.Headers;

namespace PrismApp.Services
{
    public class HttpClientBase : HttpClient
    {
        public HttpClientBase() 
            : base(new HttpClientHandler())
        {
        }

        public HttpClientBase(HttpMessageHandler handler) 
            : base(handler)
        {
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}