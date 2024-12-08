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
            serviceCollection.AddScoped<IUserApiClient, UserApiClient>();
            serviceCollection.AddScoped<IFormApiClient, FormApiClient>();
            serviceCollection.AddScoped<ICompletedFormApiClient, CompletedFormApiClient>();
            serviceCollection.AddScoped<IQuestionApiClient, QuestionApiClient>();
            serviceCollection.AddScoped<IAnswerApiClient, AnswerApiClient>();
        }

        public HttpClient CreateApiHttpClient(IServiceProvider serviceProvider, string apiBaseUrl)
        {
            var client = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
            client.BaseAddress = new Uri(apiBaseUrl);
            return client;
        }
    }
}
