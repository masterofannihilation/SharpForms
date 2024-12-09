namespace SharpForms.Web.BL.ApiClients;

public partial class AnswerApiClient
{
    public AnswerApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
