using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.IntegrationTests.Fixtures;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Enums;
using Xunit;

namespace SharpForms.Api.BL.IntegrationTests.Question
{
    public class QuestionDetailFacadeTests : FacadeTestFixture
    {
        private readonly IQuestionDetailFacade _questionDetailFacade;
        private readonly IAnswerListFacade _answerListFacade;

        public QuestionDetailFacadeTests()
        {
            _answerListFacade = new AnswerListFacade(_answerRepository, _mapper);
            _questionDetailFacade = new QuestionDetailFacade(_questionRepository, _mapper, _answerListFacade);
        }

        [Fact]
        public void Get_QuestionDetailModel_By_Id()
        {
            var model = _questionDetailFacade.GetById(new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"));
            
            Assert.NotNull(model);
            Assert.Equal("How satisfied are you with our service?", model.Text);
            Assert.NotNull(model.Answers);
            Assert.Equal(1, model.Order);
            Assert.Equal(AnswerType.Selection, model.AnswerType);
        }

        [Fact]
        public void Get_QuestionDetailModel_Returns_Null_When_Not_Found()
        {
            var questionId = Guid.NewGuid();

            var model = _questionDetailFacade.GetById(questionId);

            Assert.Null(model);
        }
        
        [Fact]
        public void Update_QuestionDetailModel()
        {
            var existingId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52");
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);

            // Update the question's text and order
            model.Text = "Updated question text";
            model.Order = 3;

            var updatedId = _questionDetailFacade.Update(model);
            
            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.Equal(3, updatedModel.Order);
            Assert.Equal("Updated question text", updatedModel.Text);
        }
        
        [Fact]
        public void Delete_QuestionDetailModel()
        {
            var existingId = new Guid("28974ee4-fda7-4357-ac75-d7a2388d51cf");

            _questionDetailFacade.Delete(existingId);

            var deletedModel = _questionDetailFacade.GetById(existingId);
            Assert.Null(deletedModel); // Ensure the question was deleted
        }
        
        [Fact]
        public void CreateOrUpdate_Creates_New_Question()
        {
            var questId = new Guid("8c25cc03-98cc-4bd8-ba9c-72e247060a0b");
            var model = new QuestionDetailModel
            {
                Id = questId,
                FormId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"),
                Order = 3,
                Text = "What is your favorite feature?",
                AnswerType = AnswerType.Text,
                FormName = "Customer Feedback"
            };

            var id = _questionDetailFacade.CreateOrUpdate(model);
            Assert.Equal(questId, id);
 
            var createdModel = _questionDetailFacade.GetById(id);
            Assert.NotNull(createdModel);
            Assert.Equal("What is your favorite feature?", createdModel.Text);
            Assert.Equal("Customer Feedback", createdModel.FormName);
        }
    }
}
