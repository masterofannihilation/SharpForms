using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Api.BL.Facades.Answer;

public class AnswerDetailFacade(
    IAnswerRepository answerRepository,
    IMapper mapper)
    : DetailModelFacadeBase<AnswerEntity, AnswerDetailModel>(answerRepository, mapper), IAnswerDetailFacade
{
    private readonly IMapper _mapper = mapper;
}
