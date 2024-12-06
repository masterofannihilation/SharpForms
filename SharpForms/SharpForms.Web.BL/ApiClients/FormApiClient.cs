namespace SharpForms.Web.BL.ApiClients;

public partial class FormApiClient
{
    public FormApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
