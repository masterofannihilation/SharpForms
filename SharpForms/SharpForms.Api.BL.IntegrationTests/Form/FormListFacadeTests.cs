using Xunit;
using SharpForms.Api.BL.Facades.Form;
using SharpForms.Api.BL.IntegrationTests.Fixtures;

namespace SharpForms.Api.BL.IntegrationTests.Form;

public class FormListFacadeTests : FacadeTestFixture
{
    private readonly IFormListFacade _facade;

    public FormListFacadeTests()
    {
        _facade = new FormListFacade(_formRepository, _mapper);
    }

    [Fact]
    public void Get_FormListModel_By_Id()
    {
        var model = _facade.GetById(new Guid("8e1c3878-d661-4a57-86b4-d30ed1592558"));

        Assert.Equal("Job Application", model!.Name);
        Assert.NotNull(model!.OpenSince);
        Assert.NotNull(model!.OpenUntil);
        Assert.Equal("Jane Doe", model!.CreatorName);
        Assert.Equal(6, model!.QuestionCount);
        Assert.Equal(2, model!.TimesCompleted);
    }

    [Fact]
    public void Filter_Forms_By_Name()
    {
        var models = _facade.SearchAllByName("Feedback");

        Assert.Single(models, m => m.Name == "Customer Feedback");
    }

    [Fact]
    public void Fitler_Forms_By_Creator()
    {
        var models = _facade.GetAllCreatedBy(new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"));

        Assert.All(models, m => Assert.Equal("Jane Doe", m.CreatorName));
    }
}
