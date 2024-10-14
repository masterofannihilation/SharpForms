using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Api.BL.Facades.Answer;

public class AnswerDetailFacade(
    AnswerRepository answerRepository,
    IMapper mapper,
    IUserListFacade userListFacade,
    IQuestionListFacade questionListFacade)
    : DetailModelFacadeBase<AnswerEntity, AnswerDetailModel>(answerRepository, mapper), IAnswerDetailFacade
{
    private readonly IMapper _mapper = mapper;

    public override AnswerDetailModel? GetById(Guid id)
    {
        var entity = answerRepository.GetById(id);
        var model = _mapper.Map<AnswerDetailModel>(entity);

        if (model == null) return null;

        model.Question = questionListFacade.GetById(entity!.QuestionId)!;
        if (entity!.CompletedForm!.UserId != null)
        {
            model.User = userListFacade.GetById(entity!.CompletedForm.UserId.Value);
        }

        model.Answer = entity.Question!.AnswerType switch
        {
            AnswerType.Text => entity.TextAnswer!,
            AnswerType.Selection => entity.SelectOption!.Option,
            AnswerType.Integer or AnswerType.Decimal => entity.NumberAnswer!.ToString()!,
            _ => model.Answer
        };

        return model;
    }
}
