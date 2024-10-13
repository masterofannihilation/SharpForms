using System;
using Xunit;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.BL.MapperProfiles;
using AutoMapper;
using SharpForms.Api.DAL.Memory.Repositories;

namespace SharpForms.Api.DAL.IntegrationTests
{
    public class RepositoryUserTests
    {
        private readonly UserRepository _userRepository;
        private readonly Storage _storage;

        public RepositoryUserTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapperProfile>());
            var mapper = config.CreateMapper();

            _storage = new Storage();
            _userRepository = new UserRepository(_storage, mapper);
        }
        [Fact]
        public void Get_Existing_User_By_Id()
        {
            var userId = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"); // Seed data

            var user = _userRepository.GetById(userId);

            Assert.NotNull(user);
            Assert.Equal("John Doe", user.Name);
            Assert.NotEmpty(user.CompletedForms);
            Assert.NotEmpty(user.CreatedForms);
        }

        [Fact]
        public void Get_Existing_User_By_Id_Check_Entities()
        {
            var userId = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"); // Seed data
            var createdForm = _storage.Forms.SingleOrDefault(form => form.Id == new Guid("8e1c3878-d661-4a57-86b4-d30ed1592558"));
            var completedForm = _storage.CompletedForms.SingleOrDefault(form => form.Id == new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"));
            Assert.NotNull(createdForm);
            Assert.NotNull(completedForm);

            var user = _userRepository.GetById(userId);

            Assert.NotNull(user);
            Assert.Contains(createdForm, user.CreatedForms);
            Assert.Contains(completedForm, user.CompletedForms);
        }

        [Fact]
        public void Get_NonExisting_User_By_Id()
        {
            var userId = new Guid("b7dc3c15-d073-4e28-b3ec-54d29bcbb00a"); // Nonexisting Guid

            var user = _userRepository.GetById(userId);

            Assert.Null(user);
        }

        [Fact]
        public void Insert_New_User()
        {
            var newUser = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Josh",
                Role = SharpForms.Common.Enums.UserRole.General
            };

            var userId = _userRepository.Insert(newUser);

            var insertedUser = _userRepository.GetById(userId);
            Assert.NotNull(insertedUser);
            Assert.Equal("Josh", insertedUser.Name);
        }

        [Fact]
        public void Update_User_Data()
        {
            var userId = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"); // Seed data
            var userToUpdate = _userRepository.GetById(userId);

            Assert.NotNull(userToUpdate);
            userToUpdate.Name = "Jane Smith";

            var updatedUserId = _userRepository.Update(userToUpdate);

            Assert.NotNull(updatedUserId);
            var updatedUser = _userRepository.GetById(updatedUserId.Value);
            Assert.NotNull(updatedUser);
            Assert.Equal("Jane Smith", updatedUser.Name);
        }

        [Fact]
        public void Remove_User()
        {
            var userId = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"); // Seed data

            _userRepository.Remove(userId);

            var user = _userRepository.GetById(userId);
            Assert.Null(user);
        }

        [Fact]
        public void Remove_User_Cascade_Remove_Data()
        {
            var userId = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"); // Seed data

            _userRepository.Remove(userId);
            var createdForm = _storage.Forms.SingleOrDefault(form => form.Id == new Guid("8e1c3878-d661-4a57-86b4-d30ed1592558"));
            var completedForm = _storage.CompletedForms.SingleOrDefault(form => form.Id == new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"));

            var user = _userRepository.GetById(userId);
            Assert.Null(user);
            Assert.NotNull(createdForm);
            Assert.NotNull(completedForm);
            Assert.Null(createdForm.CreatorId);
            Assert.Null(completedForm.UserId);
        }

        [Fact]
        public void Check_If_Existing_User_Exists()
        {
            var userId = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"); // Seed data

            var exists = _userRepository.Exists(userId);

            Assert.True(exists);
        }

        [Fact]
        public void Check_If_NonExisting_User_Exists()
        {
            var nonExistentUserId = new Guid("36ca2db8-25c0-46a1-a358-1ffdcdadb1f4"); // Nonexisting Guid

            var exists = _userRepository.Exists(nonExistentUserId);

            Assert.False(exists);
        }
    }
}
