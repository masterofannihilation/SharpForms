using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record SelectOptionEntity : EntityBase
    {
        public Guid QuestionId { get; set; }
        public QuestionEntity? Question { get; set; }
        public required string Option { get; set; }
        
        public SelectOptionEntity DeepCopy()
        {
            return new SelectOptionEntity
            {
                Id = this.Id, // Copy Id from EntityBase
                QuestionId = this.QuestionId,
                Option = this.Option,
                Question = Question?.DeepCopy() // Assuming deep copy of Question
            };
        }
    }
}
