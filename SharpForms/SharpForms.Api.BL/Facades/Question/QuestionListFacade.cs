using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Models.Question;

namespace SharpForms.Api.BL.Facades.Question;

public class QuestionListFacade(IQuestionRepository questionRepository, IMapper mapper)
    : ListModelFacadeBase<QuestionEntity, QuestionListModel>(questionRepository, mapper), IQuestionListFacade
{
    public List<QuestionListModel> GetAll(Guid? formId, string? filterText = null, string? filterDescription = null)
    {
        return questionRepository.GetAllFiltered(formId, filterText, filterDescription).Select(GetEntityToModel)
            .ToList()!;
    }
}
