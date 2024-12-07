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

    private QuestionDetailModel EditingQuestion { get; set; } = new()
    {
        Id = Guid.NewGuid(),
        FormName = string.Empty,
        Text = string.Empty,
        Options = new List<SelectOptionModel>()
    };

    private List<ValidationResult> ValidationResults { get; set; } = new();
    private bool IsEditing { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        Form = await FormApiClient.FormGetAsync(Id, "en");
        await LoadQuestions();
        ResetEditingQuestion();
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
        EditingQuestion.Options.Add(new SelectOptionModel
        {
            QuestionId = EditingQuestion.Id,
            Id = Guid.NewGuid(),
            Value = string.Empty
        });
    }

    private async Task SaveQuestion()
    {
        if (!ValidateQuestion())
        {
            return;
        }

        try
        {
            if (IsEditing)
            {
                await QuestionApiClient.QuestionPutAsync("en", EditingQuestion);
            }
            else
            {
                await QuestionApiClient.QuestionPostAsync("en", EditingQuestion);
            }
            await LoadQuestions();
            ResetEditingQuestion();
        }
        catch (ApiException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private bool ValidateQuestion()
    {
        var context = new ValidationContext(EditingQuestion);
        ValidationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(EditingQuestion, context, ValidationResults, true);

        foreach (var option in EditingQuestion.Options)
        {
            var optionContext = new ValidationContext(option);
            isValid &= Validator.TryValidateObject(option, optionContext, ValidationResults, true);
        }
        if (EditingQuestion.Options.Count < 2 && EditingQuestion.AnswerType == Common.Enums.AnswerType.Selection)
        {
            ValidationResults.Add(new ValidationResult("You must have at least 2 options.", new List<string> { "Options" }));
        }

        return isValid;
    }

    private void RemoveOption(SelectOptionModel option)
    {
        EditingQuestion.Options.Remove(option);
    }

    private async Task RemoveQuestion(Guid questionId)
    {
        await QuestionApiClient.QuestionDeleteAsync(questionId, "en");
        await LoadQuestions();
    }

    private void EditQuestion(QuestionDetailModel question)
    {
        EditingQuestion = new QuestionDetailModel
        {
            Id = question.Id,
            FormId = question.FormId,
            FormName = question.FormName,
            Text = question.Text,
            Description = question.Description,
            AnswerType = question.AnswerType,
            Order = question.Order,
            Options = new List<SelectOptionModel>(question.Options.Select(o => new SelectOptionModel
            {
                Id = o.Id,
                QuestionId = o.QuestionId,
                Value = o.Value
            }))
        };
        IsEditing = true;
    }

    private void CancelEdit()
    {
        ResetEditingQuestion();
    }

    private void ResetEditingQuestion()
    {
        EditingQuestion = new QuestionDetailModel
        {
            Id = Guid.NewGuid(),
            FormId = Id,
            FormName = Form?.Name ?? string.Empty,
            Text = string.Empty,
            Order = QuestionsAll.Count + 1,
            AnswerType = Common.Enums.AnswerType.Text,
            Options = new List<SelectOptionModel>()
        };
        IsEditing = false;
    }
}
