using SharpForms.Common.Enums;

namespace SharpForms.Common.Models.User
{
    public record UserListModel : BaseModel
    {
        public required string Name { get; set; }
        public string? PhotoUrl { get; set; }
        public required UserRole Role { get; set; }
    }
}
