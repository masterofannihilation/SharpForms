using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Api.BL.Facades.Answer;

public interface IAnswerListFacade : IListModelFacade<AnswerEntity, AnswerListModel>
{
    List<AnswerListModel> GetAll(Guid? completedFormId, Guid? questionId);
}
