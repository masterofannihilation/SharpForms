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
        ISelectOptionRepository selectOptionRepository,
        IMapper mapper)
        : DetailModelFacadeBase<SelectOptionEntity, SelectOptionModel>(selectOptionRepository, mapper), ISelectOptionFacade
    {
        public IList<SelectOptionModel> GetByQuestionId(Guid questionId)
        {
            var options = selectOptionRepository.GetAll();

            var filteredOptions = options
                .Where(option => option.QuestionId == questionId)
                .ToList();

            var filteredModels = new List<SelectOptionModel>();

            foreach (var option in filteredOptions)
                filteredModels.Add(mapper.Map<SelectOptionModel>(option));
            
            return filteredModels;
        }
    }
}
