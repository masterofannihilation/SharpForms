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
            return this with
            {
                Question = null
            };
        }
    }
}
