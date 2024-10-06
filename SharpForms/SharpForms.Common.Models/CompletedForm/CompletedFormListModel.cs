using SharpForms.Common;
using SharpForms.Common.Models.User;

namespace SharpForms.Common.Models.CompletedForm
{
    public record CompletedFormListModel : BaseModel
    {
        public UserListModel? User { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
