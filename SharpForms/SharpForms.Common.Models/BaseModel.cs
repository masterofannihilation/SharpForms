using SharpForms.Common.Models;

namespace SharpForms.Common.Models
{
    public abstract record BaseModel : IModel
    {
        public required Guid Id { get; init; }
    }
}
