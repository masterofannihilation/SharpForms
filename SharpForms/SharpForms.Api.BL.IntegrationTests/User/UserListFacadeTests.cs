using Xunit;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.BL.IntegrationTests.Fixtures;

namespace SharpForms.Api.BL.IntegrationTests.User;

public class UserListFacadeTests : FacadeTestFixture
{
    private readonly IUserListFacade _facade;

    public UserListFacadeTests()
    {
        _facade = new UserListFacade(_userRepository, _mapper);
    }

    [Fact]
    public void Get_UserListModel_By_Id()
    {
        var model = _facade.GetById(new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"));

        Assert.Equal("Jane Doe", model!.Name);
    }

    [Fact]
    public void Get_All_Users()
    {
        var models = _facade.GetAll();

        Assert.Equal(5, models.Count);
    }

    [Fact]
    public void Get_User_By_Name_Search_1()
    {
        var models = _facade.SearchAllByName("DoE");

        Assert.Equal(2, models.Count());
    }

    [Fact]
    public void Get_User_By_Name_Search_2()
    {
        var models = _facade.SearchAllByName("john");

        Assert.Contains(models, m => m.Name == "John Doe");
    }
}
