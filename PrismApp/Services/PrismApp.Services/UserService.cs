using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrismApp.Services.Interfaces;

namespace PrismApp.Services
{
    public class UserService<T> : IUserService<T>
    {
        private readonly HttpClientBase _httpClientBase;

        public UserService(HttpClientBase httpClientBase)
        {
            _httpClientBase = httpClientBase;
        }

        public async Task<IEnumerable<T>> GetUsers(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("Url is empty");
            }

            Uri.TryCreate(url, UriKind.Absolute, out var uri);

            return await GetUsers(uri);
        }

        public async Task<IEnumerable<T>> GetUsers(Uri uri)
        {
            using (HttpResponseMessage response = await _httpClientBase.GetAsync(uri))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return Array.Empty<T>();
                }

                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<T>>(result) ?? Array.Empty<T>();
            }
        }
    }
}