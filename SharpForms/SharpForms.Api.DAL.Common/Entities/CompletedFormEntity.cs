using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record CompletedFormEntity : EntityBase
    {
        public Guid FormId { get; set; }
        public FormEntity? Form { get; set; }

        public Guid? UserId { get; set; }
        public UserEntity? User { get; set; }

        public DateTime CompletedDate { get; set; }

        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
        
        // Override Equals to compare collections properly
        public bool EqualsWithAnswers(CompletedFormEntity other)
        {
            // Use the default Equals() for record's value comparison
            return Equals(other) && Answers.SequenceEqual(other.Answers);
        }
        
        public CompletedFormEntity DeepCopy()
        {
            var clonedAnswers = Answers.Select(answer => answer.DeepCopy()).ToList();
            FormEntity? clonedForm = Form?.DeepCopy();
            UserEntity? clonedUser = User?.DeepCopy();

            return new CompletedFormEntity
            {
                Id = this.Id,
                FormId = this.FormId,
                UserId = this.UserId,
                CompletedDate = this.CompletedDate,
                Answers = clonedAnswers,
                Form = clonedForm,
                User = clonedUser
            };
        }
    }
}
