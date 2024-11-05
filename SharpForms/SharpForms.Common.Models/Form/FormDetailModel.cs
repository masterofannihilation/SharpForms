using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.User;

namespace SharpForms.Common.Models.Form
{
    public record FormDetailModel : BaseModel
    {
        public required string Name { get; set; }
        public DateTime? OpenSince { get; set; }
        public DateTime? OpenUntil { get; set; }

        public UserListModel? Creator { get; set; }

        public IList<QuestionListModel> Questions { get; set; } = new List<QuestionListModel>();

        public IList<CompletedFormListModel> Completions { get; set; } = new List<CompletedFormListModel>();
    }
}
