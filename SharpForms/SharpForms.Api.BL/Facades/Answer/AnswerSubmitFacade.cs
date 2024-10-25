using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.BL.Facades.CompletedForm;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Api.BL.Facades.Answer;

public class AnswerSubmitFacade(
    IAnswerRepository answerRepository,
    ISelectOptionRepository selectOptionRepository,
    IMapper mapper,
    ICompletedFormListFacade completedFormListFacade,
    IQuestionListFacade questionListFacade)
    : DetailModelFacadeBase<AnswerEntity, AnswerSubmitModel>(answerRepository, mapper), IAnswerSubmitFacade
{
    private readonly IMapper _mapper = mapper;

    public override Guid? CreateOrUpdate(AnswerSubmitModel model)
    {
        var existingAnswer = answerRepository.GetById(model.Id);
        if (model.Id == new Guid())
            model.Id = Guid.NewGuid();

        var newAnswer = new AnswerEntity
        {
            Id = model.Id,
            QuestionId = model.QuestionId,
            Question = mapper.Map<QuestionEntity>(questionListFacade.GetById(model.QuestionId)),
            CompletedFormId = model.FilledFormId,
            CompletedForm = mapper.Map<CompletedFormEntity>(completedFormListFacade.GetById(model.FilledFormId))
        };

        switch (model.AnswerType)
        {
            case SharpForms.Common.Enums.AnswerType.Text:
                newAnswer.TextAnswer = model.TextAnswer;
                break;

            case SharpForms.Common.Enums.AnswerType.Selection:
                if (model.SelectOptionId != null)
                {
                    newAnswer.SelectOptionId = model.SelectOptionId;
                    Guid selectOptionId = (Guid)model.SelectOptionId;
                    newAnswer.SelectOption = selectOptionRepository.GetById(selectOptionId);
                }
                break;

            case SharpForms.Common.Enums.AnswerType.Integer:
            case SharpForms.Common.Enums.AnswerType.Decimal:
                newAnswer.NumberAnswer = model.NumberAnswer;
                break;
        }

        if (existingAnswer != null)
            return answerRepository.Update(newAnswer);
        else
            return answerRepository.Insert(newAnswer);
    }
}
