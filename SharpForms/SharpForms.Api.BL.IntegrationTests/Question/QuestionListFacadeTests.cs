using Xunit;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.IntegrationTests.Fixtures;

namespace SharpForms.Api.BL.IntegrationTests.Question;

public class QuestionListFacadeTests : FacadeTestFixture
{
    private readonly IQuestionListFacade _facade;

    public QuestionListFacadeTests()
    {
        _facade = new QuestionListFacade(_questionRepository, _mapper);
    }

    [Fact]
    public void Get_QuestionListModel_By_Id()
    {
        var model = _facade.GetById(new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"));

        Assert.Equal(2, model!.Order);
        Assert.Equal("Please provide additional feedback.", model!.Text);
        Assert.Null(model!.Description);
    }

    [Fact]
    public void Get_All_Questions_From_Form()
    {
        var models = _facade.GetAll(formId: new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"));

        Assert.Equal(2, models.Count);
        Assert.Single(models, m => m.Order == 1);
        Assert.Single(models, m => m.Order == 2);
    }

    [Fact]
    public void Get_Question_By_Text_Search()
    {
        var models = _facade.GetAll(null, filterText: "How");

        Assert.Equal(2, models.Count());
        Assert.Single(models, m => m.Text == "How satisfied are you with our service?");
        Assert.Single(models, m => m.Text == "How many years of experience do you have in this field?");
    }
}
