using SharpForms.Common;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Models.User;

namespace SharpForms.Common.Models.CompletedForm
{
    public record CompletedFormDetailModel : BaseModel
    {
        public Guid FormId { get; set; }
        public required string FormName { get; set; }

        public UserListModel? User { get; set; }
        public IList<AnswerListModel> Answers { get; set; } = new List<AnswerListModel>();
    }
}
