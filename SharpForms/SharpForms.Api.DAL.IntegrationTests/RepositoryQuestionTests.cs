using System;
using Xunit;
using System;
using Xunit;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.DAL.Common.Entities;
using AutoMapper;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Api.BL.MapperProfiles;

namespace SharpForms.Api.DAL.IntegrationTests
{
    public class RepositoryQuestionTests
    {
        private readonly QuestionRepository _repository;
        private readonly Storage _storage;

        public RepositoryQuestionTests()
        {
            _storage = new Storage();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<QuestionMapperProfile>());
            var mapper = new Mapper(mapperConfiguration);
            _repository = new QuestionRepository(_storage, mapper);
        }

        [Fact]
        public void Insert_New_Question()
        {
            var question = new QuestionEntity
            {
                Id = Guid.NewGuid(), Text = "Sample question", Order = 1, AnswerType = AnswerType.Text
            };
            var result = _repository.Insert(question);

            Assert.Equal(question.Id, result);
            Assert.Contains(question, _repository.GetAll());
        }

        [Fact]
        public void Get_Question_By_Id()
        {
            var questionId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"); // Seed data

            var fetchedQuestion = _repository.GetById(questionId);

            Assert.NotNull(fetchedQuestion);
            Assert.Equal("Please provide additional feedback.", fetchedQuestion.Text);
        }

        [Fact]
        public void Get_Question_By_Id_Check_Included_Entities()
        {
            var fetchedQuestion = _repository.GetById(new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1")); // Seed data

            var Answer =
                _storage.Answers.SingleOrDefault(o =>
                    o.Id == new Guid("b4505f75-f177-4076-832d-8fd1677c9a18")); // Seed data
            var Form = _storage.Forms.SingleOrDefault(o =>
                o.Id == new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8")); // Seed data
            var selectOption =
                _storage.SelectOptions.SingleOrDefault(o =>
                    o.Id == new Guid("b8189619-b77f-4038-9441-f6785db3e25b")); // Seed data

            Assert.NotNull(fetchedQuestion);
            Assert.Contains(Answer, fetchedQuestion.Answers);
            Assert.Equal(fetchedQuestion.Form, Form);
            Assert.Contains(selectOption, fetchedQuestion.Options);
        }

        [Fact]
        public void Update_Question()
        {
            var questionId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"); // Seed data
            var question = _repository.GetById(questionId);

            Assert.NotNull(question);
            question.Text = "Updated question";
            _repository.Update(question);

            var updatedQuestion = _repository.GetById(question.Id);
            Assert.NotNull(updatedQuestion);
            Assert.Equal("Updated question", updatedQuestion.Text);
        }

        [Fact]
        public void Remove_Question()
        {
            var questionId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"); // Seed data

            _repository.Remove(questionId);

            var deletedQuestion = _repository.GetById(questionId);

            Assert.Null(deletedQuestion);
        }

        [Fact]
        public void Remove_Question_Cascade_Delete_Entities()
        {
            var questionId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data

            _repository.Remove(questionId);

            var fetchedQuestion = _repository.GetById(questionId); // Seed data

            var Answer =
                _storage.Answers.SingleOrDefault(o =>
                    o.Id == new Guid("b4505f75-f177-4076-832d-8fd1677c9a18")); // Seed data
            var Form = _storage.Forms.SingleOrDefault(o =>
                o.Id == new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8")); // Seed data
            var selectOption =
                _storage.SelectOptions.SingleOrDefault(o =>
                    o.Id == new Guid("b8189619-b77f-4038-9441-f6785db3e25b")); // Seed data

            Assert.Null(fetchedQuestion);
            Assert.Null(Answer);
            Assert.Null(selectOption);
        }

        [Fact]
        public void Check_If_Question_Exists()
        {
            var questionId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"); // Seed data

            var exists = _repository.Exists(questionId);

            Assert.True(exists);
        }
    }
}
