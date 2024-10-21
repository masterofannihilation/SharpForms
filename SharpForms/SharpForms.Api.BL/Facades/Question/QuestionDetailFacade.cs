using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Models.Question;

namespace SharpForms.Api.BL.Facades.Question
{
    public class QuestionDetailFacade(
        IQuestionRepository questionRepository,
        IMapper mapper,
        IAnswerListFacade answerListFacade) 
        : DetailModelFacadeBase<QuestionEntity, QuestionDetailModel>(questionRepository, mapper), IQuestionDetailFacade
    {
        public override QuestionDetailModel? GetById(Guid id)
        {
            var entity = questionRepository.GetById(id);
            if (entity == null) return null;
            
            var model = mapper.Map<QuestionDetailModel>(entity);
            if (model == null) return null;
        
            // Load additional data related to the question
            model.Answers = answerListFacade.GetAll(null, entity.Id);
        
            return model;
        }

        public override Guid? Update(QuestionDetailModel model)
        {
            var entity = questionRepository.GetById(model.Id);
            if (entity == null) return null;

            entity.Text = model.Text;
            entity.Order = model.Order;
            entity.AnswerType = model.AnswerType;
            // entity.Answers = model.Answers;

            return questionRepository.Update(entity);
        }
    }
}
