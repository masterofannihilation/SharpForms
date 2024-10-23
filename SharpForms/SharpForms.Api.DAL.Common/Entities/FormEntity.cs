using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record FormEntity : EntityBase
    {
        public DateTime? OpenSince { get; set; }
        public DateTime? OpenUntil { get; set; }

        public Guid CreatorId { get; set; }
        public UserEntity? Creator { get; set; }

        public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
        
        public FormEntity DeepCopy()
        {
            var clonedQuestions = Questions.Select(question => question.DeepCopy()).ToList();

            return new FormEntity
            {
                Id = this.Id,
                OpenSince = this.OpenSince,
                OpenUntil = this.OpenUntil,
                CreatorId = this.CreatorId,
                Creator = Creator?.DeepCopy(),
                Questions = clonedQuestions
            };
        }
    }
}
