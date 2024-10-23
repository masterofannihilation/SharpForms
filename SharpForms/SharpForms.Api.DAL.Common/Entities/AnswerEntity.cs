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
            return new AnswerEntity
            {
                Id = this.Id, // Copy Id from EntityBase
                QuestionId = this.QuestionId,
                CompletedFormId = this.CompletedFormId,
                TextAnswer = this.TextAnswer,
                NumberAnswer = this.NumberAnswer,
                SelectOptionId = this.SelectOptionId,
                Question = Question?.DeepCopy(), // Assuming QuestionEntity has a DeepCopy method
                CompletedForm = CompletedForm?.DeepCopy(), // Assuming CompletedFormEntity has a DeepCopy method
                SelectOption = SelectOption?.DeepCopy() // Assuming SelectOptionEntity has a DeepCopy method
            };
        }
    }
}
