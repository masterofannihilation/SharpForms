using System.Net.Http.Json;
using SharpForms.Common.Models.CompletedForm;
using Xunit;

namespace SharpForms.Api.App.EndToEndTests
{
    public class ApiCompletedFormTests : BaseControllerTests
    {
        [Fact]
        public async Task GetAllCompletedForms()
        {
            var response = await Client.Value.GetAsync("/api/completedForm");

            response.EnsureSuccessStatusCode();

            var cfs = await response.Content.ReadFromJsonAsync<ICollection<CompletedFormListModel>>();
            Assert.NotNull(cfs);
            Assert.NotEmpty(cfs);
            Assert.NotNull(cfs.FirstOrDefault(cf =>
                cf.UserName == "Jane Doe" && cf.Id == new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8")));
            Assert.NotNull(cfs.FirstOrDefault(cf =>
                cf.UserName == "John Doe" && cf.Id == new Guid("07b2be2c-6fab-4303-b554-e1e742f526e0")));
        }
    }
}
