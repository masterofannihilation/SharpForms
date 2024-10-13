using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.BL.MapperProfiles;

namespace SharpForms.Api.DAL.IntegrationTests
{
    public class RepositoryAnswerTests
    {
        private readonly Storage _storage;
        private readonly AnswerRepository _repository;

        public RepositoryAnswerTests()
        {
            _storage = new Storage();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<AnswerMapperProfile>());
            var mapper = new Mapper(mapperConfiguration);

            _repository = new AnswerRepository(_storage);
        }

        [Fact]
        public void Insert_New_Answer()
        {
            var answer = new AnswerEntity { Id = Guid.NewGuid(), TextAnswer = "Sample answer" };
            var result = _repository.Insert(answer);
            var insertedAnswer = _repository.GetById(answer.Id);
            
            Assert.Equal(answer.Id, result);
            Assert.Contains(answer, _repository.GetAll());
            Assert.NotNull(insertedAnswer);
        }

        [Fact]
        public void Get_Answer_By_Id()
        {
            var fetchedAnswer = _repository.GetById(new Guid("f42f95fb-d11e-49c5-88c0-4592d6131425")); // Seed data

            Assert.NotNull(fetchedAnswer);
            Assert.Equal("Software Engineer", fetchedAnswer.TextAnswer);
        }

        [Fact]
        public void Get_Answer_By_Id_Check_Included_Entities()
        {
            var fetchedAnswer = _repository.GetById(new Guid("b4505f75-f177-4076-832d-8fd1677c9a18")); // Seed data

            var question = _storage.Questions.SingleOrDefault(o => o.Id == new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1")); // Seed data
            var completedForm = _storage.CompletedForms.SingleOrDefault(o => o.Id == new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8")); // Seed data
            var selectOption = _storage.SelectOptions.SingleOrDefault(o => o.Id == new Guid("b8189619-b77f-4038-9441-f6785db3e25b")); // Seed data

            Assert.NotNull(fetchedAnswer);
            Assert.Equal(fetchedAnswer.Question, question);
            Assert.Equal(fetchedAnswer.CompletedForm, completedForm);
            Assert.Equal(fetchedAnswer.SelectOption, selectOption);
        }

        [Fact]
        public void Update_Existing_Answer()
        {
            var answer = new AnswerEntity { Id = Guid.NewGuid(), TextAnswer = "Old answer" };
            _repository.Insert(answer);
            var oldAnswer = _repository.GetById(answer.Id);
            Assert.NotNull(oldAnswer);
            Assert.Equal(oldAnswer.TextAnswer, answer.TextAnswer);

            answer.TextAnswer = "Updated answer";
            _repository.Update(answer);

            var updatedAnswer = _repository.GetById(answer.Id);
            Assert.NotNull(updatedAnswer);
            Assert.Equal("Updated answer", updatedAnswer.TextAnswer);
        }

        [Fact]
        public void Remove_Answer()
        {
            var answerId = new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"); // Seed data
            _repository.Remove(answerId);

            var deletedAnswer = _repository.GetById(answerId);

            Assert.Null(deletedAnswer);
        }

        [Fact]
        public void Check_If_Answer_Exists()
        {
            var answerId = new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"); // Seed data

            Assert.True(_repository.Exists(answerId));
        }
    }
}
