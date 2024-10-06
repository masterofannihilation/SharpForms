using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Common.Models.Question;

namespace SharpForms.Api.BL.Facades.Question;

public class QuestionListFacade(QuestionRepository questionRepository, IMapper mapper)
    : ListModelFacadeBase<QuestionEntity, QuestionListModel>(questionRepository, mapper), IQuestionListFacade
{
}
