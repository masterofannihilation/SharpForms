using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.BL.IntegrationTests.Fixtures;
using Xunit;
namespace SharpForms.Api.BL.IntegrationTests.Answer
{
    public class AnswerDetailFacadeTests : FacadeTestFixture
    {
        private readonly IAnswerDetailFacade _answerDetailFacade;
        private readonly IUserListFacade _userListFacade;
        private readonly IQuestionListFacade _questionListFacade;

        public AnswerDetailFacadeTests()
        {
            _userListFacade = new UserListFacade(_userRepository, _mapper);
            _questionListFacade = new QuestionListFacade(_questionRepository, _mapper);
            _answerDetailFacade = new AnswerDetailFacade(_answerRepository, _mapper, _userListFacade, _questionListFacade);
        }

        [Fact]
        public void Get_AnswerDetailModel_By_Id()
        {
            var answerId = new Guid("f42f95fb-d11e-49c5-88c0-4592d6131425");
            var model = _answerDetailFacade.GetById(answerId);

            Assert.NotNull(model);
            Assert.Equal("John Doe", model.User!.Name);
            Assert.Equal("Software Engineer", model.Answer);
            Assert.Equal("What position are you applying for?", model.Question.Text);
        }

        [Fact]
        public void Delete_AnswerDetailModel()
        {
            var answerId = new Guid("f42f95fb-d11e-49c5-88c0-4592d6131425");
            var model = _answerDetailFacade.GetById(answerId);
            Assert.NotNull(model);

            _answerDetailFacade.Delete(answerId);

            model = _answerDetailFacade.GetById(answerId);
            Assert.Null(model);
        }
    }
}
