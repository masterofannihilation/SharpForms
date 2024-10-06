using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Api.BL.Facades.Answer;

public class AnswerListFacade(AnswerRepository answerRepository, IMapper mapper)
    : ListModelFacadeBase<AnswerEntity, AnswerListModel>(answerRepository, mapper), IAnswerListFacade
{
    protected override AnswerListModel? GetEntityToModel(AnswerEntity? entity)
    {
        var model = base.GetEntityToModel(entity);
        if (model == null) return null;

        var question = entity!.Question!;
        model.Answer = question.AnswerType switch
        {
            AnswerType.Text => entity.TextAnswer!,
            AnswerType.Selection => entity.SelectOption!.Option,
            AnswerType.Integer or AnswerType.Decimal => entity.NumberAnswer!.ToString()!,
            _ => model.Answer
        };

        if (entity.CompletedForm!.User != null)
        {
            model.UserName = entity.CompletedForm!.User.Name;
        }

        return model;
    }
}
