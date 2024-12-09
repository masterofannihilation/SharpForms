using Microsoft.AspNetCore.Components;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.Answer;
using SharpForms.Web.BL.ApiClients;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpForms.Common.Models.Form;

namespace SharpForms.Web.App.Pages.Form;

public partial class CompletedFormDetailPage : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    [Parameter] public Guid CompletedFormId { get; set; }
    
    [Inject] private ICompletedFormApiClient CompletedFormApiClient { get; set; } = null!;
    [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
    [Inject] private IQuestionApiClient QuestionApiClient { get; set; } = null!;
    [Inject] private IAnswerApiClient AnswerApiClient { get; set; } = null!;
    
    private FormDetailModel Form { get; set; } = null!;
    private CompletedFormDetailModel? CompletedForm { get; set; }
    private ICollection<QuestionListModel> Questions { get; set; } = [];
    private ICollection<AnswerListModel> Answers { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        Form = await FormApiClient.FormGetAsync(Id, "en");
        CompletedForm = await CompletedFormApiClient.CompletedFormGetAsync(CompletedFormId, "en");
        await LoadQuestions();

        await base.OnInitializedAsync();
    }

    private async Task LoadQuestions()
    {
        Questions = await QuestionApiClient.QuestionGetAsync(Id.ToString(), null, null, "en");
    }
}
