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
            _answerDetailFacade = new AnswerDetailFacade(_answerRepository, _mapper);
        }

        [Fact]
        public void Get_AnswerDetailModel_By_Id()
        {
            var answerId = new Guid("62f94798-1c4d-491e-8a4e-b344c1de6ef1");
            var model = _answerDetailFacade.GetById(answerId);

            Assert.NotNull(model);
            Assert.Equal("John Doe", model.User!.Name);
            Assert.Equal("Software Developer", model.Answer);
            Assert.Equal("What position are you applying for?", model.Question.Text);
        }

        [Fact]
        public void Delete_AnswerDetailModel()
        {
            var answerId = new Guid("7dbb8f17-d0d8-4a25-9bd8-927cda91f6e2");
            var model = _answerDetailFacade.GetById(answerId);
            Assert.NotNull(model);

            _answerDetailFacade.Delete(answerId);

            model = _answerDetailFacade.GetById(answerId);
            Assert.Null(model);
        }
    }
}
