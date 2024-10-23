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
        
        public CompletedFormEntity DeepCopy()
        {
            var clonedAnswers = Answers.Select(answer => answer.DeepCopy()).ToList();

            return new CompletedFormEntity
            {
                Id = this.Id,
                FormId = this.FormId,
                UserId = this.UserId,
                DateTime = this.DateTime,
                Answers = clonedAnswers
            };
        }
    }
}
