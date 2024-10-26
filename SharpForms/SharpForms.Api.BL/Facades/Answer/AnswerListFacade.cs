using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Api.BL.Facades.Answer;

public class AnswerListFacade(IAnswerRepository answerRepository, IMapper mapper)
    : ListModelFacadeBase<AnswerEntity, AnswerListModel>(answerRepository, mapper), IAnswerListFacade
{
    protected override IEnumerable<AnswerEntity> GetAllFetchMethod()
    {
        return answerRepository.GetAllWithUser(null, null).AsEnumerable();
    }

    public List<AnswerListModel> GetAll(Guid? completedFormId, Guid? questionId)
    {
        return answerRepository.GetAllWithUser(completedFormId, questionId).AsEnumerable().Select(this.GetEntityToModel)
            .ToList()!;
    }
}
