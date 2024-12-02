namespace SharpForms.Web.BL.ApiClients;

public partial class UserApiClient
{
    public UserApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
