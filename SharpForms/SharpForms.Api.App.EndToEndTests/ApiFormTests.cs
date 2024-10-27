using System.Net;
using System.Net.Http.Json;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.User;
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

        [Fact]
        public async Task GetFormDetails()
        {
            var formId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"); // Seed data for Form1
            var response = await Client.Value.GetAsync($"/api/form/{formId}");

            response.EnsureSuccessStatusCode();

            var formDetail = await response.Content.ReadFromJsonAsync<FormDetailModel>();
            Assert.NotNull(formDetail);
            Assert.Equal(formId, formDetail.Id);
            Assert.Equal("Customer Feedback", formDetail.Name);
            Assert.NotEmpty(formDetail.Questions);
            Assert.NotNull(formDetail.Creator);
            Assert.Equal("John Doe", formDetail.Creator!.Name);
            
            Assert.NotEmpty(formDetail.Completions);
            Assert.All(formDetail.Completions, c => Assert.NotNull(c.UserName));
        }

        [Fact]
        public async Task CreateForm()
        {
            var newForm = new FormDetailModel
            {
                Id = new Guid(),
                Name = "New Survey",
                OpenSince = DateTime.Now.AddDays(-1),
                OpenUntil = DateTime.Now.AddDays(10),
                Creator = new UserListModel
                {
                    Id = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"), // User2
                    Name = "Jane Doe"
                },
                Questions = []
            };

            var response = await Client.Value.PostAsJsonAsync("/api/form", newForm);

            response.EnsureSuccessStatusCode();

            var createdFormId = await response.Content.ReadFromJsonAsync<Guid?>();

            response = await Client.Value.GetAsync($"/api/form/{createdFormId}");
            response.EnsureSuccessStatusCode();
            var createdForm = await response.Content.ReadFromJsonAsync<FormDetailModel>();
            Assert.NotNull(createdForm);
            Assert.Equal("New Survey", createdForm.Name);
            Assert.Equal("Jane Doe", createdForm.Creator!.Name);
            Assert.Empty(createdForm.Questions);
        }

        [Fact]
        public async Task UpdateForm()
        {
            var formId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"); // Seed data for Form1
            var updatedForm = new FormDetailModel
            {
                Id = formId,
                Name = "Updated Survey",
                OpenSince = DateTime.Now.AddDays(-2),
                OpenUntil = DateTime.Now.AddDays(5),
                Creator = new UserListModel
                {
                    Id = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"), // User2
                    Name = "Jane Doe"
                },
                Questions = []
            };

            var response = await Client.Value.PutAsJsonAsync($"/api/form/", updatedForm);

            response.EnsureSuccessStatusCode();

            var formResponse = await Client.Value.GetAsync($"/api/form/{formId}");
            formResponse.EnsureSuccessStatusCode();
            var formDetail = await formResponse.Content.ReadFromJsonAsync<FormDetailModel>();

            Assert.NotNull(formDetail);
            Assert.Equal("Updated Survey", formDetail.Name);
            Assert.Equal("Jane Doe", formDetail.Creator!.Name);
        }

        [Fact]
        public async Task DeleteForm()
        {
            var formId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"); // Seed data for Form1

            var response = await Client.Value.DeleteAsync($"/api/form/{formId}");

            response.EnsureSuccessStatusCode();

            response = await Client.Value.GetAsync($"/api/form/{formId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
