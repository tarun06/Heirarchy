using System;
using Microsoft.Extensions.Configuration;

namespace PrismApp.Services
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("Configuration can not be null");
            }

            _configuration = configuration;

            ServiceUrl = GetServiceUrl("Endpoints:Url");
        }

        public Uri ServiceUrl { get; private set; }

        private Uri GetServiceUrl(string key)
        {
            return Uri.TryCreate(_configuration[key], UriKind.Absolute, out var uri) ? uri : null;
        }
    }
}