using Xunit;
using SharpForms.Api.BL.Facades.CompletedForm;
using SharpForms.Api.BL.IntegrationTests.Fixtures;

namespace SharpForms.Api.BL.IntegrationTests.CompletedForm;

public class CompletedFormListFacadeTests : FacadeTestFixture
{
    private readonly ICompletedFormListFacade _facade;

    public CompletedFormListFacadeTests()
    {
        _facade = new CompletedFormListFacade(_completedFormRepository, _mapper);
    }

    [Fact]
    public void Get_CompletedFormListModel_By_Id()
    {
        var model = _facade.GetById(new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"));

        Assert.Equal("Jane Doe", model!.UserName);
        Assert.NotNull(model!.CompletedDate);
    }

    [Fact]
    public void GetAll()
    {
        var models = _facade.GetAll();

        Assert.Equal(2, models.Count);
        Assert.Contains(models, m => m.UserName == "Jane Doe");
        Assert.Contains(models, m => m.UserName == "John Doe");
    }

    [Fact]
    public void GetAll_CompletedFormListModels_By_UserId()
    {
        var models = _facade.GetAllCompletionsMadeByUser(new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"));

        Assert.Single(models);
        Assert.All(models, m => Assert.Equal("John Doe", m.UserName));
    }

    [Fact]
    public void GetAll_CompletedFormListModels_By_FormId()
    {
        var models = _facade.GetAllCopletionsOfForm(new Guid("8e1c3878-d661-4a57-86b4-d30ed1592558"));

        Assert.Single(models);
        Assert.Contains(models, m => m.Id == new Guid("07b2be2c-6fab-4303-b554-e1e742f526e0"));
    }
}
