namespace SharpForms.Common.Models.SelectOption
{
    public record SelectOptionModel : BaseModel
    {
        public required Guid QuestionId { get; set; }
        public required string Value { get; set; }
    }
}
