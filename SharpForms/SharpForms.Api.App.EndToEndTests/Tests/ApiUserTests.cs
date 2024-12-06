using System.Data;
using System.Net;
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

        [Fact]
        public async Task GetUserDetails()
        {
            var userId = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"); // Seed data John Doe
            var response = await Client.Value.GetAsync($"/api/user/{userId}");

            response.EnsureSuccessStatusCode();

            var userDetail = await response.Content.ReadFromJsonAsync<UserDetailModel>();
            Assert.NotNull(userDetail);
            Assert.Equal("John Doe", userDetail.Name);
            Assert.NotEmpty(userDetail.CreatedForms);
            Assert.NotEmpty(userDetail.CompletedForms);
        }

        [Fact]
        public async Task UpdateUserDetails()
        {
            var userId = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"); // Seed data John Doe
            var updateUser = new UserDetailModel
            {
                Id = userId,
                Name = "John Updated",
                Role = Common.Enums.UserRole.Admin,
                PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/50/John_and_Jane_Doe_Headstones.jpg/640px-John_and_Jane_Doe_Headstones.jpg"
            };

            var response = await Client.Value.PutAsJsonAsync($"/api/user", updateUser);

            response.EnsureSuccessStatusCode();

            var updatedUserId = await response.Content.ReadFromJsonAsync<Guid?>();
            Assert.Equal(updatedUserId, userId);

            response = await Client.Value.GetAsync($"/api/user/{userId}");
            response.EnsureSuccessStatusCode();
            var userDetailUpdated = await response.Content.ReadFromJsonAsync<UserDetailModel>();
            Assert.NotNull(userDetailUpdated);

            Assert.Equal("John Updated", userDetailUpdated.Name);
            Assert.Equal(Common.Enums.UserRole.Admin, userDetailUpdated.Role);
            Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/thumb/5/50/John_and_Jane_Doe_Headstones.jpg/640px-John_and_Jane_Doe_Headstones.jpg", userDetailUpdated.PhotoUrl);
        }

        [Fact]
        public async Task CreateUser()
        {
            var newUser = new UserDetailModel
            {
                Id = new Guid(),
                Name = "New User",
                Role = Common.Enums.UserRole.General,
                PhotoUrl = "https://cdn-icons-png.flaticon.com/512/166/166260.png"
            };

            var response = await Client.Value.PostAsJsonAsync("/api/user", newUser);

            response.EnsureSuccessStatusCode();

            var createdUserId = await response.Content.ReadFromJsonAsync<Guid?>();

            response = await Client.Value.GetAsync($"/api/user/{createdUserId}");
            response.EnsureSuccessStatusCode();
            var userDetailNew = await response.Content.ReadFromJsonAsync<UserDetailModel>();
            Assert.NotNull(userDetailNew);

            Assert.Equal("New User", userDetailNew.Name);
            Assert.Equal(Common.Enums.UserRole.General, userDetailNew.Role);
            Assert.Equal("https://cdn-icons-png.flaticon.com/512/166/166260.png", userDetailNew.PhotoUrl);
        }

        [Fact]
        public async Task DeleteUser()
        {
            var userId = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783");

            var response = await Client.Value.DeleteAsync($"/api/user/{userId}");

            response.EnsureSuccessStatusCode();

            response = await Client.Value.GetAsync($"/api/user/{userId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
