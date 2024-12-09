namespace SharpForms.Web.BL.ApiClients;

public partial class CompletedFormApiClient
{
    public CompletedFormApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
