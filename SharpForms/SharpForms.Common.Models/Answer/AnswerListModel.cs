namespace SharpForms.Common.Models.Answer
{
    public record AnswerListModel : BaseModel
    {
        public int Order { get; set; } // Order of the question/answer in the form.
        public string? UserName { get; set; }
        public required string Answer { get; set; }
    }
}
