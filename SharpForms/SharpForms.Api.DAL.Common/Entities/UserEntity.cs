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
            return this with
            {
                CompletedForms = [],
                CreatedForms = []
            };
        }
    }
}
