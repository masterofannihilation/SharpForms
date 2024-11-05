using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.BL.Facades.CompletedForm;
using SharpForms.Api.BL.Facades.Form;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.BL.IntegrationTests.Fixtures;
using SharpForms.Common.Models.User;
using Xunit;
namespace SharpForms.Api.BL.IntegrationTests.User
{
    public class UserDetailFacadeTests : FacadeTestFixture
    {
        private readonly IUserDetailFacade _userDetailFacade;
        private readonly ICompletedFormListFacade _completedFormListFacade;
        private readonly IFormListFacade _formListFacade;
        private readonly IUserListFacade _userListFacade;

        public UserDetailFacadeTests()
        {
            _completedFormListFacade = new CompletedFormListFacade(_completedFormRepository, _mapper);
            _formListFacade = new FormListFacade(_formRepository, _mapper);
            _userDetailFacade = new UserDetailFacade(_userRepository, _mapper, _completedFormListFacade, _formListFacade);
            _userListFacade = new UserListFacade(_userRepository, _mapper);
        }

        [Fact]
        public void Get_UserDetailModel_By_Id()
        {
            var userId = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"); // Seed data John Doe
            var userModel = _userDetailFacade.GetById(userId); 
            var completedForms = _completedFormListFacade.GetAllCompletionsMadeByUser(userId);
            var createdForms = _formListFacade.GetAllCreatedBy(userId);

            Assert.NotNull(userModel);
            Assert.Equal("John Doe", userModel.Name);
            Assert.Equal(Common.Enums.UserRole.General, userModel.Role);
            Assert.Equal(completedForms, userModel.CompletedForms);
            Assert.Equal(createdForms, userModel.CreatedForms);
        }

        [Fact]
        public void Create_New_UserDetailModel()
        {
            var userId = new Guid("0f97bc03-98cc-4bd8-ba9c-72e247060a0b"); // New Guid
            var model = new UserDetailModel
            {
                Id = userId,
                Name = "Joseph Carrot",
                Role = Common.Enums.UserRole.General
            };

            var insertedId = _userDetailFacade.Create(model);
            Assert.Equal(userId, insertedId);

            var insertedModel = _userDetailFacade.GetById(userId);
            var completedForms = _completedFormListFacade.GetAllCompletionsMadeByUser(userId);
            var createdForms = _formListFacade.GetAllCreatedBy(userId);

            Assert.NotNull(insertedModel);
            Assert.Equal("Joseph Carrot", insertedModel.Name);
            Assert.Equal(Common.Enums.UserRole.General, insertedModel.Role);
            Assert.Equal(completedForms, insertedModel.CompletedForms);
            Assert.Equal(createdForms, insertedModel.CreatedForms);
        }

        [Fact]
        public void Update_UserDetailModel()
        {
            var userId = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"); // Seed data John Doe
            var userModel = _userDetailFacade.GetById(userId);

            Assert.NotNull(userModel);
            userModel.Name = "Joseph Carrot";
            userModel.Role = Common.Enums.UserRole.Admin;

            var insertedId = _userDetailFacade.Update(userModel);

            var newU = _userListFacade.SearchAllByName("Joseph Carrot");
            Assert.NotEmpty(newU);

            var updatedModel = _userDetailFacade.GetById(userId);
            var completedForms = _completedFormListFacade.GetAllCompletionsMadeByUser(userId);
            var createdForms = _formListFacade.GetAllCreatedBy(userId);

            Assert.NotNull(updatedModel);
            Assert.Equal(userId, updatedModel.Id);
            Assert.Equal("Joseph Carrot", updatedModel.Name);
            Assert.Equal(Common.Enums.UserRole.Admin, updatedModel.Role);
            Assert.Equal(completedForms, updatedModel.CompletedForms);
            Assert.Equal(createdForms, updatedModel.CreatedForms);
        }

        [Fact]
        public void Delete_UserDetailModel() {
            var userId = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"); // Seed data John Doe
            var model = _userDetailFacade.GetById(userId);
            Assert.NotNull(model);

            _userDetailFacade.Delete(userId);

            model = _userDetailFacade.GetById(userId);
            Assert.Null(model);
        }
    }
}
