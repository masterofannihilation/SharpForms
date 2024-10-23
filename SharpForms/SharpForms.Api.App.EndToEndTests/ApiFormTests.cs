using System.Net.Http.Json;
using SharpForms.Common.Models.Form;
using Xunit;

namespace SharpForms.Api.App.EndToEndTests
{
    public class ApiFormTests : BaseControllerTests
    {
        [Fact]
        public async Task GetAllForms()
        {
            var response = await Client.Value.GetAsync("/api/form");

            response.EnsureSuccessStatusCode();

            var forms = await response.Content.ReadFromJsonAsync<ICollection<FormListModel>>();
            Assert.NotNull(forms);
            Assert.NotEmpty(forms);

            Assert.NotNull(forms.FirstOrDefault(f => f.Name == "Customer Feedback"));
            Assert.NotNull(forms.FirstOrDefault(f => f.Name == "Job Application"));
        }

        [Fact]
        public async Task SearchFormsByName()
        {
            var response = await Client.Value.GetAsync("/api/form?name=cust");

            response.EnsureSuccessStatusCode();

            var forms = await response.Content.ReadFromJsonAsync<ICollection<FormListModel>>();
            Assert.NotNull(forms);
            Assert.NotNull(forms.FirstOrDefault(f => f.Name == "Customer Feedback"));
        }
    }
}
