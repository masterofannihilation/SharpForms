using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Question;

namespace SharpForms.Api.BL.Facades.Question;

public interface IQuestionListFacade : IListModelFacade<QuestionEntity, QuestionListModel>
{
}
