using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.BL.Facades.CompletedForm;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.BL.IntegrationTests.Fixtures;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Models.CompletedForm;
using Xunit;
namespace SharpForms.Api.BL.IntegrationTests.User
{
    public class CompletedFormDetailFacadeTests : FacadeTestFixture
    {
        private readonly ICompletedFormDetailFacade _completedFormDetailFacade;
        private readonly ICompletedFormListFacade _completedFormListFacade;
        private readonly IUserListFacade _userListFacade;
        private readonly IAnswerListFacade _answerListFacade;

        public CompletedFormDetailFacadeTests()
        {
            _userListFacade = new UserListFacade(_userRepository, _mapper);
            _answerListFacade = new AnswerListFacade(_answerRepository, _mapper);
            _completedFormListFacade = new CompletedFormListFacade(_completedFormRepository, _mapper);
            _completedFormDetailFacade = new CompletedFormDetailFacade(_completedFormRepository, _mapper, _userListFacade, _answerListFacade);
        }

        [Fact]
        public void Get_CompletedFormDetailModel_By_Id()
        {
            var formId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"); // Seed data form 1
            var formModel = _completedFormDetailFacade.GetById(formId);
            var user = _userListFacade.GetById(new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783")); // Seed data Jane Doe
            var answer1 = _answerListFacade.GetById(new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"));
            var answer2 = _answerListFacade.GetById(new Guid("cbe36665-4fe4-4b9e-ae57-0b2e288c4d74"));

            Assert.NotNull(formModel);
            Assert.Equal("Customer Feedback", formModel.FormName);
            Assert.Equal(user, formModel.User);
            Assert.Contains(answer1, formModel.Answers);
            Assert.Contains(answer2, formModel.Answers);
        }

        [Fact]
        public void Create_New_CompletedFormDetailModel()
        {
            var formId = new Guid("4a3ef238-f14a-4f0d-b2b7-8eafcbd3737c"); // New Guid
            var model = new CompletedFormDetailModel
            {
                Id = formId,
                FormId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"), // Seeds form 1
                FormName = "Customer Feedback",
                Answers = new List<AnswerListModel>()
            };

            var insertedId = _completedFormDetailFacade.Create(model);
            Assert.Equal(formId, insertedId);

            var insertedModel = _completedFormDetailFacade.GetById(formId);
           
            Assert.NotNull(insertedModel);
            Assert.Equal("Customer Feedback", insertedModel.FormName);
            Assert.Equal(new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"), insertedModel.FormId);
            Assert.Empty(insertedModel.Answers);
        }

        [Fact]
        public void Delete_CompletedFormDetailModel()
        {
            var formId = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"); // Seed data form 1
            var formModel = _completedFormDetailFacade.GetById(formId);
            Assert.NotNull(formModel);

            _completedFormDetailFacade.Delete(formId);

            formModel = _completedFormDetailFacade.GetById(formId);
            Assert.Null(formModel);
        }
    }
}
