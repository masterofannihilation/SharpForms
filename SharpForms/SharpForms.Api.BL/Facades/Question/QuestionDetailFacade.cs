using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;

namespace SharpForms.Api.BL.Facades.Question
{
    public class QuestionDetailFacade(
        IQuestionRepository questionRepository,
        IMapper mapper,
        IAnswerListFacade answerListFacade,
        IAnswerDetailFacade answerDetailFacade,
        ISelectOptionFacade selectOptionFacade) 
        : DetailModelFacadeBase<QuestionEntity, QuestionDetailModel>(questionRepository, mapper), IQuestionDetailFacade
    {
        public override QuestionDetailModel? GetById(Guid id)
        {
            var entity = questionRepository.GetById(id);
            if (entity == null) return null;
            
            var model = mapper.Map<QuestionDetailModel>(entity);
            if (model == null) return null;
        
            model.Answers = answerListFacade.GetAll(null, entity.Id);
            model.Options = selectOptionFacade.GetByQuestionId(entity.Id);
            
            return model;
        }

        public override Guid? Update(QuestionDetailModel model)
        {
            var entity = questionRepository.GetById(model.Id);
            if (entity == null) return null;

            entity.Text = model.Text;
            entity.Order = model.Order;

            // If answerType is not Selection, update entity and return
            if (model.AnswerType != entity.AnswerType) {
                entity.AnswerType = model.AnswerType;
                if (model.AnswerType != SharpForms.Common.Enums.AnswerType.Selection)
                {
                    entity.AnswerType = model.AnswerType;
                    var relatedAnswers = answerListFacade.GetAll(null, model.Id);
                    foreach (var relatedAnswer in relatedAnswers)
                        answerDetailFacade.Delete(relatedAnswer.Id);
                    return questionRepository.Update(entity);
                }
            }

            var oldOptions = selectOptionFacade.GetByQuestionId(model.Id);

            // Check if any options were removed
            var removedOptions = oldOptions
            .Where(optEntity => !model.Options.Any(optModel => optModel.Id == optEntity.Id))
            .ToList();

            if (removedOptions.Any())
            {
                // Remove all Answers to this question
                var answerList = answerListFacade.GetAll(null, model.Id);
                foreach (var answer in answerList)
                    answerDetailFacade.Delete(answer.Id);

                // Delete removed selectOptions
                foreach (var removedOption in removedOptions)
                    selectOptionFacade.Delete(removedOption.Id);
            }

            //Check if any options were updated
            var updatedOptions = model.Options
            .Join(oldOptions,
                optModelNew => optModelNew.Id,
                optModelOld => optModelOld.Id,
                (optModelNew, optModelOld) => new { NewModel = optModelNew, OldModel = optModelOld })
            .Where(m => m.NewModel.Value != m.OldModel.Value)
            .ToList();

            if (updatedOptions.Any())
            {
                // Remove all answers to this question
                var answerList = answerListFacade.GetAll(null, model.Id);
                foreach (var answer in answerList)
                    answerDetailFacade.Delete(answer.Id);

                // Remove old selectOptions
                foreach (var option in oldOptions)
                    selectOptionFacade.Delete(option.Id);

                // Add the updated selectOptions
                foreach (var newOption in model.Options)
                    selectOptionFacade.Create(newOption);
            }

            // Check if any new options were added
            var newOptions = model.Options
            .Where(optModel => !oldOptions.Any(optEntity => optEntity.Id == optModel.Id))
            .ToList();

            if (newOptions.Count > 0)
                foreach (var newOption in newOptions)
                    selectOptionFacade.Create(newOption);

            return questionRepository.Update(entity);
        }
    }
}
