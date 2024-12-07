using System.ComponentModel.DataAnnotations;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Models.SelectOption;

namespace SharpForms.Common.Models.Question
{
    public record QuestionDetailModel : BaseModel
    {
        public Guid FormId { get; set; }
        public required string FormName { get; set; }

        public int Order { get; set; }
        
        [Required(ErrorMessage = "Question text is required")]
        public required string Text { get; set; }
        public string? Description { get; set; }
        public AnswerType AnswerType { get; set; }

        public double? MinNumber { get; set; }
        public double? MaxNumber { get; set; }

        public IList<SelectOptionModel> Options { get; set; } = new List<SelectOptionModel>();

        public IList<AnswerListModel> Answers { get; set; } = new List<AnswerListModel>();
    }
}
