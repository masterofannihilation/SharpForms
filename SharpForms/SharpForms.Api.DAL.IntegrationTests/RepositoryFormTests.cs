using SharpForms.Api.DAL.Memory;
using SharpForms.Api.DAL.Common.Entities;
using AutoMapper;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Api.BL.MapperProfiles;
using FluentAssertions;

namespace SharpForms.Api.DAL.IntegrationTests
{
    public class RepositoryFormTests : RepositoryTestFixture
    {

        public RepositoryFormTests() { }

        [Fact]
        public void Insert_Form()
        {
            var form = new FormEntity
            {
                Id = Guid.NewGuid(), Name = "Sample form", OpenSince = DateTime.Now, OpenUntil = DateTime.Now.AddDays(10), CreatorId = Guid.NewGuid()
            };
            var result = _formRepository.Insert(form);
            Assert.Equal(form.Id, result);

            var forms = _formRepository.GetAll();
            var newForm = _formRepository.GetById(form.Id);

            Assert.NotNull(forms);
            Assert.NotNull(newForm);

            forms.Should().ContainEquivalentOf(form);
            newForm.Should().BeEquivalentTo(form);
        }

        [Fact]
        public void Get_Form_By_Id()
        {
            var formId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"); //Seed data

            var fetchedForm = _formRepository.GetById(formId);

            Assert.NotNull(fetchedForm);
            Assert.Equal("Customer Feedback", fetchedForm.Name);
        }

        [Fact]
        public void Get_Form_By_Id_Check_Entities()
        {
            // Seed data
            var FormId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8");
            var Creator = _storage.Users.SingleOrDefault(o => o.Id == new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"));
            var Question1 =
                _storage.Questions.SingleOrDefault(o => o.Id == new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"));
            var Question2 =
                _storage.Questions.SingleOrDefault(o => o.Id == new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"));
            var CompletedForm =
                _storage.CompletedForms.SingleOrDefault(o => o.Id == new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"));

            var fetchedForm = _formRepository.GetById(FormId);

            Assert.NotNull(fetchedForm);
            Assert.Equal(Creator, fetchedForm.Creator);
            Assert.Contains(Question1, fetchedForm.Questions);
            Assert.Contains(Question2, fetchedForm.Questions);
            Assert.Contains(CompletedForm, fetchedForm.Completions);
        }

        [Fact]
        public void Update_Form()
        {
            var formId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"); //Seed data
            var fetchedForm = _formRepository.GetById(formId);

            Assert.NotNull(fetchedForm);
            fetchedForm.Name = "Updated form";
            _formRepository.Update(fetchedForm);

            var updatedForm = _formRepository.GetById(fetchedForm.Id);
            Assert.NotNull(updatedForm);
            Assert.Equal("Updated form", updatedForm.Name);
        }

        [Fact]
        public void Remove_Form()
        {
            var formId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"); //Seed data
            _formRepository.Remove(formId);

            var deletedForm = _formRepository.GetById(formId);

            Assert.Null(deletedForm);
        }

        [Fact]
        public void Remove_Form_Cascade_Delete_Entities()
        {
            var FormId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8");
            _formRepository.Remove(FormId);

            // Seed data
            var Question1 =
                _storage.Questions.SingleOrDefault(o => o.Id == new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"));
            var Question2 =
                _storage.Questions.SingleOrDefault(o => o.Id == new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"));
            var CompletedForm =
                _storage.CompletedForms.SingleOrDefault(o => o.Id == new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"));
            var Answer1 =
                _storage.Answers.SingleOrDefault(o => o.Id == new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"));
            var Answer2 =
                _storage.Answers.SingleOrDefault(o => o.Id == new Guid("cbe36665-4fe4-4b9e-ae57-0b2e288c4d74"));
            var SelectOption2 =
                _storage.Answers.SingleOrDefault(o => o.Id == new Guid("00fc620d-c945-412f-9555-08e3cb076884"));

            var fetchedForm = _formRepository.GetById(FormId);

            Assert.Null(fetchedForm);
            Assert.Null(Question1);
            Assert.Null(Question2);
            Assert.Null(CompletedForm);
            Assert.Null(Answer1);
            Assert.Null(Answer2);
            Assert.Null(SelectOption2);
        }

        [Fact]
        public void Check_If_Form_Exists()
        {
            var formId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"); //Seed data

            var exists = _formRepository.Exists(formId);

            Assert.True(exists);
        }
    }
}
