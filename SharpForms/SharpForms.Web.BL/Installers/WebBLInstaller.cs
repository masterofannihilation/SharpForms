using System;
using System.Net.Http;
using SharpForms.Common.BL.Facades;
using Microsoft.Extensions.DependencyInjection;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.BL.Installers
{
    public class WebBLInstaller
    {
        public void Install(IServiceCollection serviceCollection, string apiBaseUrl)
        {
            serviceCollection.AddTransient<IUserApiClient, UserApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new UserApiClient(client, apiBaseUrl);
            });
        }

        public HttpClient CreateApiHttpClient(IServiceProvider serviceProvider, string apiBaseUrl)
        {
            var client = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
            client.BaseAddress = new Uri(apiBaseUrl);
            return client;
        }
    }
}
