using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record QuestionEntity : EntityBase
    {
        public Guid FormId { get; set; }
        public FormEntity? Form { get; set; }

        public int Order { get; set; }
        public string? Text { get; set; }
        public string? Description { get; set; }
        public AnswerType? AnswerType { get; set; }

        public double? MinNumber { get; set; }
        public double? MaxNumber { get; set; }

        // Used when AnswerType=Selection. 
        public ICollection<SelectOptionEntity> Options { get; set; } = new List<SelectOptionEntity>();

        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

        public QuestionEntity DeepCopy()
        {
            var clonedOptions = Options.Select(option => option.DeepCopy()).ToList();
            var clonedAnswers = Answers.Select(answer => answer.DeepCopy()).ToList();

            return new QuestionEntity
            {
                Id = this.Id,
                FormId = this.FormId,
                Order = this.Order,
                Text = this.Text,
                Description = this.Description,
                AnswerType = this.AnswerType,
                MinNumber = this.MinNumber,
                MaxNumber = this.MaxNumber,
                Options = clonedOptions,
                Answers = clonedAnswers
            };
        }
    }
}
