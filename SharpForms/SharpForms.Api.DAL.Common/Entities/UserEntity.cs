using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Common.Entities
{
    public record UserEntity : EntityBase
    {
        public required string Name { get; set; }
        public string? PhotoUrl { get; set; }
        public required UserRole Role { get; set; }

        public ICollection<CompletedFormEntity> CompletedForms { get; set; } = new List<CompletedFormEntity>();
        public ICollection<FormEntity> CreatedForms { get; set; } = new List<FormEntity>();
        
        public UserEntity DeepCopy()
        {
            var clonedCompletedForms = CompletedForms.Select(form => form.DeepCopy()).ToList();
            var clonedCreatedForms = CreatedForms.Select(form => form.DeepCopy()).ToList();

            return new UserEntity
            {
                Id = this.Id, // Copy Id from EntityBase
                Name = this.Name,
                PhotoUrl = this.PhotoUrl,
                Role = this.Role,
                CompletedForms = clonedCompletedForms,
                CreatedForms = clonedCreatedForms
            };
        }
    }
}
