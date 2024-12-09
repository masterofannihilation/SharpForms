using System.ComponentModel.DataAnnotations;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Common.Enums;

namespace SharpForms.Common.Models.Answer
{
    /// <summary>
    /// This model should be used for submitting or editing the answer.
    /// </summary>
    public record AnswerSubmitModel : BaseModel
    {
        public Guid FilledFormId { get; set; } // ID of CompletedFormEntity

        public Guid QuestionId { get; set; }
        public int QuestionOrder { get; set; }
        public required string Text { get; set; }
        public string? Description { get; set; }

        public AnswerType AnswerType { get; set; }

        public double? MinNumber { get; set; }
        public double? MaxNumber { get; set; }
        public IList<SelectOptionModel> Options { get; set; } = new List<SelectOptionModel>();

        // Answer value depending on the type of Question (AnswerType)
        [Required(ErrorMessage = "This field is required.")]
        public string? TextAnswer { get; set; }
        [Required(ErrorMessage = "This field is required.")]

        public double? NumberAnswer { get; set; }
        [Required(ErrorMessage = "This field is required.")]

        public Guid? SelectOptionId { get; set; }
    }
}
