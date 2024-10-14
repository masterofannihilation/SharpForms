using Xunit;
using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.BL.IntegrationTests.Fixtures;

namespace SharpForms.Api.BL.IntegrationTests.Answer;

public class AnswerListFacadeTests : FacadeTestFixture
{
    private readonly IAnswerListFacade _facade;

    public AnswerListFacadeTests()
    {
        _facade = new AnswerListFacade(_answerRepository, _mapper);
    }

    [Fact]
    public void Get_AnswerListModel_By_Id()
    {
        var model = _facade.GetById(new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"));

        Assert.Equal(1, model!.Order);
        Assert.Equal("Jane Doe", model!.UserName);
        Assert.Equal("Very Satisfied", model!.Answer);
    }

    [Fact]
    public void Get_All_Answers_Of_Completed_Form()
    {
        var models = _facade.GetAll(new Guid("07b2be2c-6fab-4303-b554-e1e742f526e0"), null);

        Assert.Equal(2, models.Count);
        Assert.True(models.All(m => m.UserName == "John Doe"));
    }

    [Fact]
    public void Get_All_Answers_To_Question()
    {
        var models = _facade.GetAll(null, new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"));

        Assert.Single(models);
        Assert.True(models.All(m => m.Order == 1));
    }
}
