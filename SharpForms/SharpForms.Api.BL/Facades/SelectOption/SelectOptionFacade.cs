using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Api.DAL.Memory.Repositories;

namespace SharpForms.Api.BL.Facades.Question
{
    public class SelectOptionFacade(
        IQuestionRepository questionRepository,
        ISelectOptionRepository selectOptionRepository,
        IMapper mapper)
        : DetailModelFacadeBase<SelectOptionEntity, SelectOptionModel>(selectOptionRepository, mapper), ISelectOptionFacade
    {
        public override SelectOptionModel? GetById(Guid id)
        {
            var entity = selectOptionRepository.GetById(id);
            if(entity == null)
                return null;

            var model = new SelectOptionModel
            {
                Id = entity.Id,
                QuestionId = entity.QuestionId,
                Value = entity.Option
            };

            return model;
        }
        public override Guid Create(SelectOptionModel model)
        {
            if(model.Id == new Guid())
                model.Id = Guid.NewGuid();

            var entity = new SelectOptionEntity
            {
                Id = model.Id,
                QuestionId = model.QuestionId,
                Option = model.Value
            };

            return selectOptionRepository.Insert(entity);
        }
        public override Guid? Update(SelectOptionModel model)
        {
            var entity = selectOptionRepository.GetById(model.Id);
            if (entity == null) return null;

            entity.Option = model.Value;

            return selectOptionRepository.Update(entity);
        }

        public IList<SelectOptionModel> GetByQuestionId(Guid questionId)
        {
            var options = selectOptionRepository.GetAll();

            var filteredOptions = options
                .Where(option => option.QuestionId == questionId)
                .ToList();

            var filteredModels = new List<SelectOptionModel>();

            foreach (var option in filteredOptions)
            {
                var optionModel = new SelectOptionModel
                {
                    Id = option.Id,
                    QuestionId = option.QuestionId,
                    Value = option.Option,
                };
                filteredModels.Add(optionModel);
            }
            return filteredModels;
        }
    }
}
