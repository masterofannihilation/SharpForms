using System.Net;
using System.Net.Http.Json;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.User;
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

        [Fact]
        public async Task GetCompletedFormDetails()
        {
            var completedFormId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"); // Seed data for CompletedForm1
            var response = await Client.Value.GetAsync($"/api/completedForm/{completedFormId}");

            response.EnsureSuccessStatusCode();

            var completedFormDetail = await response.Content.ReadFromJsonAsync<CompletedFormDetailModel>();
            Assert.NotNull(completedFormDetail);
            Assert.Equal(completedFormId, completedFormDetail.Id);
            Assert.Equal("Customer Feedback", completedFormDetail.FormName);
            Assert.NotEmpty(completedFormDetail.Answers);
        }
        
        [Fact]
        public async Task CreateCompletedForm()
        {
            var newCompletedForm = new CompletedFormDetailModel
            {
                Id = new Guid(),
                FormId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"), // form1
                FormName = "",
                User = new UserListModel
                {
                    Id = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"), // User2
                    Name = "Jane Doe"
                },
                Answers = []
            };

            var response = await Client.Value.PostAsJsonAsync("/api/completedform", newCompletedForm);

            response.EnsureSuccessStatusCode();

            var createdCompletedFormId = await response.Content.ReadFromJsonAsync<Guid?>();

            response = await Client.Value.GetAsync($"/api/completedform/{createdCompletedFormId}");
            response.EnsureSuccessStatusCode();
            var completedFormDetailNew = await response.Content.ReadFromJsonAsync<CompletedFormDetailModel>();
            Assert.NotNull(completedFormDetailNew);
            Assert.Equal("Customer Feedback", completedFormDetailNew.FormName);
            Assert.Empty(completedFormDetailNew.Answers);
        }


        [Fact]
        public async Task DeleteCompletedForm()
        {
            var completedFormId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"); // Seed data for CompletedForm1

            var response = await Client.Value.DeleteAsync($"/api/completedform/{completedFormId}");

            response.EnsureSuccessStatusCode();

            response = await Client.Value.GetAsync($"/api/completedform/{completedFormId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
