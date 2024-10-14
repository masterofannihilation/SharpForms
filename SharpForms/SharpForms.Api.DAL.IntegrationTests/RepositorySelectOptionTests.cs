using System;
using System.Linq;
using Xunit;
using AutoMapper;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Api.DAL.Common.Entities;
using Moq;
using SharpForms.Api.BL.MapperProfiles;


namespace SharpForms.Api.DAL.IntegrationTests
{
    public class RepositorySelectOptionTests
    {
        private readonly Storage _storage;
        private readonly SelectOptionRepository _selectOptionRepository;

        public RepositorySelectOptionTests()
        {
            _storage = new Storage();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<SelectOptionMapperProfile>());
            var mapper = config.CreateMapper();

            _selectOptionRepository = new SelectOptionRepository(_storage, mapper);
        }

        [Fact]
        public void Get_SelectOption_By_Id_Check_Entities()
        {
            var selectOptionId = new Guid("b8189619-b77f-4038-9441-f6785db3e25b"); // Seed data
            var questionId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data
            var question = _storage.Questions.FirstOrDefault(q => q.Id == questionId);
            Assert.NotNull(question);

            var result = _selectOptionRepository.GetById(selectOptionId);

            Assert.NotNull(result);
            Assert.Equal(questionId, result.QuestionId);
            Assert.NotNull(result.Question);
            Assert.Equal(question, result.Question);
        }

        [Fact]
        public void Retrieve_SelectOptions_For_A_Question()
        {
            var question =
                _storage.Questions.FirstOrDefault(q => q.AnswerType == SharpForms.Common.Enums.AnswerType.Selection);
            Assert.NotNull(question);

            var options = _selectOptionRepository.GetAll().Where(o => o.QuestionId == question.Id).ToList();

            Assert.NotEmpty(options);
            Assert.All(options, o => Assert.Equal(question.Id, o.QuestionId));
        }

        [Fact]
        public void Insert_New_SelectOption()
        {
            var newSelectOption = new SelectOptionEntity
            {
                Id = new Guid("4ac86a35-a15f-426b-ba80-bf73263b2ed9"),
                QuestionId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"), // Seed data
                Option = "Terrible"
            };

            var insertedId = _selectOptionRepository.Insert(newSelectOption);

            var insertedOption = _storage.SelectOptions.SingleOrDefault(o => o.Id == insertedId);
            Assert.NotNull(insertedOption);
            Assert.Equal(newSelectOption.Option, insertedOption.Option);
            Assert.Equal(newSelectOption.QuestionId, insertedOption.QuestionId);
        }

        [Fact]
        public void Update_SelectOption()
        {
            var SelectOptionId = new Guid("b8189619-b77f-4038-9441-f6785db3e25b"); // Seed data
            var selectOption = _selectOptionRepository.GetById(SelectOptionId);
            Assert.NotNull(selectOption);

            var updatedOption = new SelectOptionEntity
            {
                Id = selectOption.Id, QuestionId = selectOption.QuestionId, Option = "Updated Option"
            };

            var updatedId = _selectOptionRepository.Update(updatedOption);

            Assert.NotNull(updatedId);
            var updatedSelectOption = _selectOptionRepository.GetById(selectOption.Id);
            Assert.NotNull(updatedSelectOption);
            Assert.Equal("Updated Option", updatedSelectOption.Option);
        }

        [Fact]
        public void Remove_SelectOption()
        {
            var selectOptionId = new Guid("b8189619-b77f-4038-9441-f6785db3e25b"); // Seed data

            _selectOptionRepository.Remove(selectOptionId);

            var removedOption = _storage.SelectOptions.SingleOrDefault(o => o.Id == selectOptionId);
            Assert.Null(removedOption);
        }

        [Fact]
        public void Check_If_SelectOption_Exists()
        {
            var selectOptionId = new Guid("00fc620d-c945-412f-9555-08e3cb076884"); // Seed data

            var exists = _selectOptionRepository.Exists(selectOptionId);

            Assert.True(exists);
        }
    }
}
