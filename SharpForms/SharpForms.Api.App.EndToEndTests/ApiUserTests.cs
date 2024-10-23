using System.Net.Http.Json;
using SharpForms.Common.Models.User;
using Xunit;

namespace SharpForms.Api.App.EndToEndTests
{
    public class ApiUserTests : BaseControllerTests
    {
        [Fact]
        public async Task GetAllUsers()
        {
            var response = await Client.Value.GetAsync("/api/user");

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<ICollection<UserListModel>>();
            Assert.NotNull(users);
            Assert.NotEmpty(users);

            Assert.NotNull(users.FirstOrDefault(u => u.Name == "John Doe"));
            Assert.NotNull(users.FirstOrDefault(u => u.Name == "Jane Doe"));
        }

        [Fact]
        public async Task SearchUsersByName()
        {
            var response = await Client.Value.GetAsync("/api/user?name=john");

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<ICollection<UserListModel>>();
            Assert.NotNull(users);
            Assert.NotNull(users.FirstOrDefault(u => u.Name == "John Doe"));
            Assert.Null(users.FirstOrDefault(u => u.Name == "Jane Doe"));
        }
    }
}
