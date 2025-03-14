using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record FormEntity : EntityBase
    {
        public required string Name { get; set; }

        public DateTime? OpenSince { get; set; }
        public DateTime? OpenUntil { get; set; }

        public Guid? CreatorId { get; set; }
        public UserEntity? Creator { get; set; }

        public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
        public ICollection<CompletedFormEntity> Completions { get; set; } = new List<CompletedFormEntity>();
        
        public FormEntity DeepCopy()
        {
            return this with
            {
                Creator = null,
                Questions = [],
                Completions = [],
            };
        }

    }
}
