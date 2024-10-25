using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record AnswerEntity : EntityBase
    {
        public Guid QuestionId { get; set; }
        public QuestionEntity? Question { get; set; }

        public Guid CompletedFormId { get; set; }
        public CompletedFormEntity? CompletedForm { get; set; }

        public string? TextAnswer { get; set; }
        public double? NumberAnswer { get; set; }
        public Guid? SelectOptionId { get; set; }
        public SelectOptionEntity? SelectOption { get; set; }
        
        public AnswerEntity DeepCopy()
        {
            return this with
            {
                Question = null,
                CompletedForm = null,
                SelectOption = null
            };
        }
    }
}
