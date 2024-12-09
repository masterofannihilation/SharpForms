using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.Form;

public partial class FromFillPage : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
    [Inject] private IQuestionApiClient QuestionApiClient { get; set; } = null!;
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;
    [Inject] private IAnswerApiClient AnswerApiClient { get; set; } = null!;
    [Inject] private ICompletedFormApiClient CompletedFormApiClient { get; set; } = null!;
    private UserListModel? CurrentUser { get; set; }
    private ICollection<QuestionDetailModel> Questions { get; set; } = new List<QuestionDetailModel>();
    private FormDetailModel? Form { get; set; }
    private CompletedFormDetailModel CompletedForm { get; set; } = new()
    { Id = Guid.NewGuid(), FormName = String.Empty, };
    private ICollection<AnswerSubmitModel> Answers { get; set; } = new List<AnswerSubmitModel>();
    private bool IsUserAuthenticated { get; set; }
    private string? ValidationMessage { get; set; }


    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var user = authState.User;
        IsUserAuthenticated = user.Identity?.IsAuthenticated ?? false;

        if (IsUserAuthenticated)
        {
            var username = user.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                var userList = await UserApiClient.UserGetAsync(username, "en");
                CurrentUser = userList.FirstOrDefault();
            }
        }

        Form = await FormApiClient.FormGetAsync(Id, "en");
        await LoadQuestions();
        InitializeAnswers();
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

    private void InitCompletedForm()
    {
        CompletedForm.FormId = Form!.Id;
        CompletedForm.FormName = Form!.Name;
        CompletedForm.User = CurrentUser;
    }
    private void InitializeAnswers()
    {
        InitCompletedForm();
        Answers = Questions.Select(question => new AnswerSubmitModel
        {
            Id = Guid.NewGuid(),
            FilledFormId = CompletedForm.Id,
            QuestionId = question.Id,
            QuestionOrder = question.Order,
            Text = string.Empty,
            AnswerType = question.AnswerType,
            Options = question.Options,
            TextAnswer = string.Empty,
            NumberAnswer = null,
            SelectOptionId = null,
        }).ToList();
    }

    private async Task HandleValidSubmit()
    {
        if (AreAllQuestionsAnswered())
        {
            foreach (var answer in Answers)
            {
                await AnswerApiClient.AnswerPostAsync("en", answer);
            }

            await CompletedFormApiClient.CompletedFormPostAsync("en", CompletedForm);

            NavigationManager.NavigateTo("/forms");
        }
        else
        {
            ValidationMessage = "Please answer all questions before submitting the form.";
        }
    }

    private void SelectOption(Guid questionId, Guid optionId)
    {
        var answer = Answers.First(a => a.QuestionId == questionId);
        answer.SelectOptionId = optionId;
    }

    private bool AreAllQuestionsAnswered()
    {
        var unansweredQuestions = Answers.Where(answer =>
            (answer.AnswerType == AnswerType.Text && string.IsNullOrWhiteSpace(answer.TextAnswer)) ||
            (answer.AnswerType == AnswerType.Integer && !answer.NumberAnswer.HasValue) ||
            (answer.AnswerType == AnswerType.Selection && !answer.SelectOptionId.HasValue)).ToList();

        if (unansweredQuestions.Any())
        {
            return false;
        }

        return true;
    }

}
