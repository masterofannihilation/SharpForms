namespace SharpForms.Common.Models.Question
{
    public record QuestionListModel : BaseModel
    {
        public int Order { get; set; }
        public required string Text { get; set; }
        public string? Description { get; set; }
    }
}
