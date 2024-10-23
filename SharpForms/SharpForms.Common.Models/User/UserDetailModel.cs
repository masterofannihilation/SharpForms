using SharpForms.Common.Enums;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Form;

namespace SharpForms.Common.Models.User
{
    public record UserDetailModel : BaseModel
    {
        public required string Name { get; set; }
        public string? PhotoUrl { get; set; }
        public required UserRole Role { get; set; }

        public IList<CompletedFormListModel> CompletedForms { get; set; } = new List<CompletedFormListModel>();
        public IList<FormListModel> CreatedForms { get; set; } = new List<FormListModel>();
    }
}
