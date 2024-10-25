using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.BL.MapperProfiles;
using FluentAssertions;

namespace SharpForms.Api.DAL.IntegrationTests
{
    public class RepositoryCompletedFormTests : RepositoryTestFixture
    {

        public RepositoryCompletedFormTests() { }

        [Fact]
        public void Insert_CompletedForm()
        {
            var completedForm = new CompletedFormEntity { Id = Guid.NewGuid(), CompletedDate = DateTime.Now };
            var result = _completedFormRepository.Insert(completedForm);

            Assert.Equal(completedForm.Id, result);
            var forms = _completedFormRepository.GetAll();
            
            var newForm = _completedFormRepository.GetById(completedForm.Id);

            Assert.NotNull(forms);
            Assert.NotNull(newForm);

            forms.Should().ContainEquivalentOf(completedForm);
            newForm.Should().BeEquivalentTo(completedForm);
        }
        
        [Fact]
        public void Get_CompletedForm_By_Id()
        {
            var completedFormId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"); // Seed data
            var FormUserId = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"); // Seed data

            var fetchedForm = _completedFormRepository.GetById(completedFormId);

            Assert.NotNull(fetchedForm);
            Assert.Equal(FormUserId, fetchedForm.UserId);
        }

        [Fact]
        public void Get_CompletedForm_By_Id_Check_Entities()
        {
            // Seed data
            var completedFormId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8");
            var User = _storage.Users.SingleOrDefault(o => o.Id == new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"));
            var Answer1 =
                _storage.Answers.SingleOrDefault(o => o.Id == new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"));
            var Answer2 =
                _storage.Answers.SingleOrDefault(o => o.Id == new Guid("cbe36665-4fe4-4b9e-ae57-0b2e288c4d74"));
            var Form = _storage.Forms.SingleOrDefault(o => o.Id == new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"));

            var fetchedForm = _completedFormRepository.GetById(completedFormId);

            Assert.NotNull(fetchedForm);
            Assert.Equal(User, fetchedForm.User);
            Assert.Contains(Answer1, fetchedForm.Answers);
            Assert.Contains(Answer2, fetchedForm.Answers);
            Assert.Equal(Form, fetchedForm.Form);
        }

        [Fact]
        public void Update_Existing_CompletedForm()
        {
            var completedForm = new CompletedFormEntity { Id = Guid.NewGuid(), CompletedDate = DateTime.Now };
            _completedFormRepository.Insert(completedForm);

            completedForm.CompletedDate = DateTime.Now.AddDays(2);
            _completedFormRepository.Update(completedForm);

            var updatedForm = _completedFormRepository.GetById(completedForm.Id);

            Assert.NotNull(updatedForm);
            Assert.Equal(completedForm.CompletedDate, updatedForm.CompletedDate);
        }

        [Fact]
        public void Remove_CompletedForm()
        {
            var formId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"); // Seed data
            _completedFormRepository.Remove(formId);

            var deletedForm = _completedFormRepository.GetById(formId);

            Assert.Null(deletedForm);
        }

        [Fact]
        public void Remove_CompletedForm_Cascade_Delete_Answers()
        {
            var formId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"); // Seed data
            _completedFormRepository.Remove(formId);

            var deletedForm = _completedFormRepository.GetById(formId);
            var Answer1 =
                _storage.Answers.SingleOrDefault(o => o.Id == new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"));
            var Answer2 =
                _storage.Answers.SingleOrDefault(o => o.Id == new Guid("cbe36665-4fe4-4b9e-ae57-0b2e288c4d74"));

            Assert.Null(deletedForm);
            Assert.Null(Answer1);
            Assert.Null(Answer2);
        }

        [Fact]
        public void Check_If_CompletedForm_Exists()
        {
            var formId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"); // Seed data

            var exists = _completedFormRepository.Exists(formId);

            Assert.True(exists);
        }
    }
}
