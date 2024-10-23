using SharpForms.Common;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.User;

namespace SharpForms.Common.Models.Answer
{
    /// <summary>
    /// This model is only used for viewing the details of the submitted answer.
    /// </summary>
    public record AnswerDetailModel : BaseModel
    {
        public required UserListModel? User { get; set; }
        public required QuestionListModel Question { get; set; }
        public required string Answer { get; set; }
    }
}
