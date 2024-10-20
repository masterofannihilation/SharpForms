using Xunit;
using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.BL.IntegrationTests.Fixtures;
using SharpForms.Common.Models.Answer;
using SharpForms.Api.BL.Facades.CompletedForm;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.Facades.User;

namespace SharpForms.Api.BL.IntegrationTests.Answer
{
    public class AnswerSubmitFacadeTests : FacadeTestFixture
    {
        private readonly IAnswerDetailFacade _answerDetailFacade;
        private readonly IAnswerSubmitFacade _answerSubmitFacade;
        private readonly ICompletedFormListFacade _completedFormListFacade;
        private readonly IQuestionListFacade _questionListFacade;
        private readonly IUserListFacade _userListFacade;

        public AnswerSubmitFacadeTests()
        {
            _userListFacade = new UserListFacade(_userRepository, _mapper);
            _questionListFacade = new QuestionListFacade(_questionRepository, _mapper);
            _answerDetailFacade = new AnswerDetailFacade(_answerRepository, _mapper, _userListFacade, _questionListFacade);
            _completedFormListFacade = new CompletedFormListFacade(_completedFormRepository, _mapper);
            _answerSubmitFacade = new AnswerSubmitFacade(_answerRepository, _selectOptionRepository, _mapper, _completedFormListFacade, _questionListFacade);
        }

        [Fact]
        public void Submit_New_Answer()
        {
            var model = new AnswerSubmitModel
            {
                Id = new Guid("528d2f26-a9ad-4b5f-8f9e-d8ae3e24d2c1"), // New guid
                QuestionId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"), // Seed data question 2
                FilledFormId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"), //Seed data Form 1 
                TextAnswer = "Test answer",
                Text = "Please provide additional feedback."
            };

            _answerSubmitFacade.CreateOrUpdate(model);

            var createdAnswer = _answerRepository.GetById(model.Id);
            Assert.NotNull(createdAnswer);
            Assert.Equal("Test answer", createdAnswer.TextAnswer);
            Assert.Equal(new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"), createdAnswer.CompletedFormId);
            Assert.Equal(new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"), createdAnswer.Question!.Id);

            var result = _answerDetailFacade.GetById(model.Id);

            Assert.NotNull(result);
            Assert.Equal("Test answer", model.TextAnswer);
            Assert.Equal(new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"), model.FilledFormId);
            Assert.NotNull(result.Question);
            Assert.Equal(new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"), result.Question.Id);

        }

        [Fact]
        public void Update_Existing_Answer()
        {
            var model = new AnswerSubmitModel
            {
                Id = new Guid("cbe36665-4fe4-4b9e-ae57-0b2e288c4d74"), // Seed data answer 2
                QuestionId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"), // Seed data question 2
                FilledFormId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"), //Seed data Form 1 
                TextAnswer = "Test answer",
                Text = "Please provide additional feedback."
            };

            _answerSubmitFacade.CreateOrUpdate(model);

            var createdAnswer = _answerRepository.GetById(model.Id);
            Assert.NotNull(createdAnswer);
            Assert.Equal("Test answer", createdAnswer.TextAnswer);
            Assert.Equal(new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"), createdAnswer.CompletedFormId);
            Assert.Equal(new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"), createdAnswer.Question!.Id);

            var result = _answerDetailFacade.GetById(model.Id);

            Assert.NotNull(result);
            Assert.Equal("Test answer", model.TextAnswer);
            Assert.Equal(new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"), model.FilledFormId);
            Assert.NotNull(result.Question);
            Assert.Equal(new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"), result.Question.Id);
        }
    }
}
