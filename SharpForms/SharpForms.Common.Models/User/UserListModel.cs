namespace SharpForms.Common.Models.User
{
    public record UserListModel : BaseModel
    {
        public required string Name { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
