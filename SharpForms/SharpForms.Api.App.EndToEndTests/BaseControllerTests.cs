namespace SharpForms.Api.App.EndToEndTests
{
    public class BaseControllerTests : IAsyncDisposable
    {
        private readonly SharpFormsApiApplicationFactory _application;
        protected readonly Lazy<HttpClient> Client;

        internal BaseControllerTests()
        {
            _application = new SharpFormsApiApplicationFactory();
            Client = new Lazy<HttpClient>(_application.CreateClient());
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}
