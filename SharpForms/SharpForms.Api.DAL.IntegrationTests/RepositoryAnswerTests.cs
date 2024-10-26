using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.BL.MapperProfiles;
using FluentAssertions;

namespace SharpForms.Api.DAL.IntegrationTests
{
    public class RepositoryAnswerTests : RepositoryTestFixture
    {

        public RepositoryAnswerTests() { }


        [Fact]
        public void Insert_New_Answer()
        {
            var answer = new AnswerEntity { Id = Guid.NewGuid(), TextAnswer = "Sample answer" };
            var result = _answerRepository.Insert(answer);
            var answers = _answerRepository.GetAll();
            var newAnswer = _answerRepository.GetById(answer.Id);

            Assert.NotNull(answers);
            Assert.NotNull(newAnswer);

            answers.Should().ContainEquivalentOf(answer);
            newAnswer.Should().BeEquivalentTo(answer);
        }

        [Fact]
        public void Get_Answer_By_Id()
        {
            var fetchedAnswer = _answerRepository.GetById(new Guid("62f94798-1c4d-491e-8a4e-b344c1de6ef1")); // Seed data

            Assert.NotNull(fetchedAnswer);
            Assert.Equal("Software Developer", fetchedAnswer.TextAnswer);
        }

        [Fact]
        public void Get_Answer_By_Id_Check_Included_Entities()
        {
            var fetchedAnswer = _answerRepository.GetById(new Guid("b4505f75-f177-4076-832d-8fd1677c9a18")); // Seed data

            var question =
                _storage.Questions.SingleOrDefault(o =>
                    o.Id == new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1")); // Seed data
            var completedForm =
                _storage.CompletedForms.SingleOrDefault(o =>
                    o.Id == new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8")); // Seed data
            var selectOption =
                _storage.SelectOptions.SingleOrDefault(o =>
                    o.Id == new Guid("b8189619-b77f-4038-9441-f6785db3e25b")); // Seed data

            Assert.NotNull(fetchedAnswer);
            Assert.Equal(fetchedAnswer.Question, question);
            Assert.Equal(fetchedAnswer.CompletedForm, completedForm);
            Assert.Equal(fetchedAnswer.SelectOption, selectOption);
        }

        [Fact]
        public void Update_Existing_Answer()
        {
            var answer = new AnswerEntity { Id = Guid.NewGuid(), TextAnswer = "Old answer" };
            _answerRepository.Insert(answer);
            var oldAnswer = _answerRepository.GetById(answer.Id);
            Assert.NotNull(oldAnswer);
            Assert.Equal(oldAnswer.TextAnswer, answer.TextAnswer);

            answer.TextAnswer = "Updated answer";
            _answerRepository.Update(answer);

            var updatedAnswer = _answerRepository.GetById(answer.Id);
            Assert.NotNull(updatedAnswer);
            Assert.Equal("Updated answer", updatedAnswer.TextAnswer);
        }

        [Fact]
        public void Remove_Answer()
        {
            var answerId = new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"); // Seed data
            _answerRepository.Remove(answerId);

            var deletedAnswer = _answerRepository.GetById(answerId);

            Assert.Null(deletedAnswer);
        }

        [Fact]
        public void Check_If_Answer_Exists()
        {
            var answerId = new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"); // Seed data

            Assert.True(_answerRepository.Exists(answerId));
        }
    }
}
