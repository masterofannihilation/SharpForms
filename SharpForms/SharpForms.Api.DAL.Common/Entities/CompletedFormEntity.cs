using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record CompletedFormEntity : EntityBase
    {
        public Guid FormId { get; set; }
        public FormEntity? Form { get; set; }

        public Guid UserId { get; set; }
        public UserEntity? User { get; set; }

        public DateTime? DateTime { get; set; }

        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
    }
}
