namespace SharpForms.Common.Models.Form
{
    public record FormListModel : BaseModel
    {
        public required string Name { get; set; }

        public DateTime? OpenSince { get; set; }
        public DateTime? OpenUntil { get; set; }

        public required string? CreatorName { get; set; }

        public int QuestionCount { get; set; }
        public int TimesCompleted { get; set; }
    }
}
