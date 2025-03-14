using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record QuestionEntity : EntityBase
    {
        public Guid FormId { get; set; }
        public FormEntity? Form { get; set; }

        public int Order { get; set; }
        public required string Text { get; set; }
        public string? Description { get; set; }
        public AnswerType AnswerType { get; set; }

        public double? MinNumber { get; set; }
        public double? MaxNumber { get; set; }

        // Used when AnswerType=Selection. 
        public ICollection<SelectOptionEntity> Options { get; set; } = new List<SelectOptionEntity>();

        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

        public QuestionEntity DeepCopy()
        {
            return this with
            {
                Form = null,
                Options = [],
                Answers = []
            };
        }
    }
}
