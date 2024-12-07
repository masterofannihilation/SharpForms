using System.ComponentModel.DataAnnotations;

namespace SharpForms.Common.Models.SelectOption
{
    public record SelectOptionModel : BaseModel
    {
        public required Guid QuestionId { get; set; }
        [Required(ErrorMessage = "Option value is required")]
        public required string Value { get; set; }
    }
}
