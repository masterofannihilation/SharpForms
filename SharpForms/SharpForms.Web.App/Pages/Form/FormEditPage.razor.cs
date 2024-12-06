using Microsoft.AspNetCore.Components;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Web.BL.ApiClients;
using System.ComponentModel.DataAnnotations;

namespace SharpForms.Web.App.Pages.Form;

public partial class FormEditPage : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    [Inject] private IQuestionApiClient QuestionApiClient { get; set; } = null!;
    [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
    private FormDetailModel? Form { get; set; }
    private List<QuestionDetailModel> QuestionsAll { get; set; } = new();

    private QuestionDetailModel NewQuestion { get; set; } = new()
    {
        Id = Guid.NewGuid(),
        FormName = string.Empty,
        Text = string.Empty,
        Options = new List<SelectOptionModel>()
    };

    private List<ValidationResult> ValidationResults { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Form = await FormApiClient.FormGetAsync(Id, "en");
        await LoadQuestions();
        NewQuestion.FormId = Id;
        NewQuestion.FormName = Form?.Name ?? string.Empty;
        NewQuestion.Order = QuestionsAll.Count + 1;
        await base.OnInitializedAsync();
    }

    private async Task LoadQuestions()
    {
        var questionList = await QuestionApiClient.QuestionGetAsync(Id.ToString(), null, null, "en");
        QuestionsAll = new List<QuestionDetailModel>();

        foreach (var question in questionList)
        {
            var questionDetail = await QuestionApiClient.QuestionGetAsync(question.Id, "en");
            QuestionsAll.Add(questionDetail);
        }
    }

    private void AddOption()
    {
        NewQuestion.Options.Add(new SelectOptionModel
        {
            QuestionId = NewQuestion.Id,
            Id = Guid.NewGuid(),
            Value = string.Empty
        });
    }

    private async Task AddQuestion()
    {
        if (!ValidateNewQuestion())
        {
            return;
        }

        try
        {
            await QuestionApiClient.QuestionPostAsync("en", NewQuestion);
            await LoadQuestions();

            // Reset NewQuestion after posting
            NewQuestion = new QuestionDetailModel
            {
                Id = Guid.NewGuid(),
                FormId = Id,
                FormName = Form?.Name ?? string.Empty,
                Text = string.Empty,
                Order = QuestionsAll.Count + 1,
                AnswerType = Common.Enums.AnswerType.Text,
                Options = new List<SelectOptionModel>()
            };
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private bool ValidateNewQuestion()
    {
        var context = new ValidationContext(NewQuestion);
        ValidationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(NewQuestion, context, ValidationResults, true);

        foreach (var option in NewQuestion.Options)
        {
            var optionContext = new ValidationContext(option);
            isValid &= Validator.TryValidateObject(option, optionContext, ValidationResults, true);
        }
        if (NewQuestion.Options.Count < 2 && NewQuestion.AnswerType == Common.Enums.AnswerType.Selection)
        {
            ValidationResults.Add(new ValidationResult("You must have at least 2 options.", new List<string> { "Options" }));
        }

        return isValid;
    }

    private void RemoveOption(SelectOptionModel option)
    {
        NewQuestion.Options.Remove(option);
    }

    private async Task RemoveQuestion(Guid questionId)
    {
        await QuestionApiClient.QuestionDeleteAsync(questionId, "en");
        await LoadQuestions();
    }
}
