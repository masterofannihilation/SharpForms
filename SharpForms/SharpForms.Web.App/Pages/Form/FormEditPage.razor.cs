using Microsoft.AspNetCore.Components;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Web.BL.ApiClients;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.JSInterop;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Web.App.Pages.Form;

public partial class FormEditPage : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    [Inject] private IQuestionApiClient QuestionApiClient { get; set; } = null!;
    [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
    private FormDetailModel Form { get; set; }
    private ICollection<QuestionDetailModel> Questions { get; set; } = new List<QuestionDetailModel>();

    private QuestionDetailModel Question { get; set; } = new()
    {
        Id = Guid.NewGuid(), FormName = string.Empty, Text = string.Empty
    };

    private IList<SelectOptionModel> Options { get; set; } = new List<SelectOptionModel>();

    private bool IsEditing { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Form = await FormApiClient.FormGetAsync(Id, "en");
        await LoadQuestions();
        InitQuestion();
        await base.OnInitializedAsync();
    }

    private async Task LoadQuestions()
    {
        var questionList = await QuestionApiClient.QuestionGetAsync(Id.ToString(), null, null, "en");
        Questions = new List<QuestionDetailModel>();
        foreach (var question in questionList)
        {
            var questionDetail = await QuestionApiClient.QuestionGetAsync(question.Id, "en");
            Questions.Add(questionDetail);
        }
    }

    private void InitQuestion()
    {
        IsEditing = false;
        Question.Id = Guid.NewGuid();
        Question.FormId = Form.Id;
        Question.FormName = Form.Name;
        Question.Text = string.Empty;
        Question.Order = Questions.Count + 1;
        Question.Description = String.Empty;
        Question.AnswerType = Common.Enums.AnswerType.Text;
        Question.MinNumber = null;
        Question.MaxNumber = null;
        Question.Options = new List<SelectOptionModel>();
        Question.Answers = new List<AnswerListModel>();
    }

    private async Task SubmitQuestion()
    {
        if (ValidateQuestion())
        {
            if (IsEditing)
            {
                if (Question.AnswerType == Common.Enums.AnswerType.Selection ||
                    Question.AnswerType == Common.Enums.AnswerType.Text)
                {
                    Question.MinNumber = null;
                    Question.MaxNumber = null;
                }
                await QuestionApiClient.QuestionPutAsync("en", Question);
            }
            else
            {
                await QuestionApiClient.QuestionPostAsync("en", Question);
            }
            await LoadQuestions();
            InitQuestion();
            // Close the modal
            // await InvokeAsync(() => StateHasChanged());
        }
    }

    private async Task DeleteQuestion(Guid questionId)
    {
        await QuestionApiClient.QuestionDeleteAsync(questionId, "en");
        await LoadQuestions();
    }

    private void EditQuestion(QuestionDetailModel question)
    {
        Question = new QuestionDetailModel
        {
            Id = question.Id,
            FormId = question.FormId,
            FormName = question.FormName,
            Text = question.Text,
            Description = question.Description,
            AnswerType = question.AnswerType,
            MinNumber = question.MinNumber,
            MaxNumber = question.MaxNumber,
            Order = question.Order,
            Options = new List<SelectOptionModel>(question.Options.Select(o => new SelectOptionModel
            {
                Id = o.Id, QuestionId = o.QuestionId, Value = o.Value
            }))
        };
        IsEditing = true;
        
    }

    private void AddOption()
    {
        Question.Options.Add(new SelectOptionModel
        {
            Id = Guid.NewGuid(), QuestionId = Question.Id, Value = string.Empty
        });
    }

    private void DeleteOption(SelectOptionModel option)
    {
        Question.Options.Remove(option);
    }

    private List<ValidationResult> ValidationResults { get; set; } = new();

    private bool ValidateQuestion()
    {
        var context = new ValidationContext(Question);
        ValidationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(Question, context, ValidationResults, true);

        foreach (var option in Question.Options)
        {
            var optionContext = new ValidationContext(option);
            isValid &= Validator.TryValidateObject(option, optionContext, ValidationResults, true);
        }

        if (Question.Options.Count < 2 && Question.AnswerType == Common.Enums.AnswerType.Selection)
        {
            ValidationResults.Add(new ValidationResult("You must have at least 2 options.",
                new List<string> { "Options" }));
        }
        
        if ((Question.AnswerType == Common.Enums.AnswerType.Integer || Question.AnswerType == Common.Enums.AnswerType.Decimal) &&
            Question.MinNumber.HasValue && Question.MaxNumber.HasValue && Question.MinNumber >= Question.MaxNumber)
        {
            ValidationResults.Add(new ValidationResult("Min value must be less than Max value.",
                new List<string> { "MinNumber", "MaxNumber" }));
            isValid = false;
        }

        return isValid;
    }
}
