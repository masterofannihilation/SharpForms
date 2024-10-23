using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.DAL.Memory.Repositories
{
    public class SelectOptionRepository : ISelectOptionRepository
    {
        private readonly IList<SelectOptionEntity> _selectOptions;
        private readonly IList<QuestionEntity> _questions;
        private readonly IMapper _mapper;

        public SelectOptionRepository(Storage storage, IMapper mapper)
        {
            _selectOptions = storage.SelectOptions;
            _questions = storage.Questions;
            _mapper = mapper;
        }

        public IList<SelectOptionEntity> GetAll()
        {
            return _selectOptions.Select(option => option.DeepCopy()).ToList();
        }

        public SelectOptionEntity? GetById(Guid id)
        {
            var selectOptionEntity = _selectOptions.SingleOrDefault(option => option.Id == id);

            if (selectOptionEntity == null) return null;

            // Load related data and return a deep copy
            var clonedOption = selectOptionEntity.DeepCopy();
            clonedOption.Question = _questions.SingleOrDefault(q => q.Id == clonedOption.QuestionId);
            return clonedOption;
        }

        public Guid Insert(SelectOptionEntity selectOption)
        {
            var optionCopy = selectOption.DeepCopy();
            _selectOptions.Add(optionCopy);
            return optionCopy.Id;
        }

        public Guid? Update(SelectOptionEntity selectOption)
        {
            var existingOption = _selectOptions.SingleOrDefault(option => option.Id == selectOption.Id);

            if (existingOption == null) return null;

            _mapper.Map(selectOption, existingOption); // Update option with new values
            return existingOption.Id;
        }

        public void Remove(Guid id)
        {
            var optionToRemove = _selectOptions.SingleOrDefault(option => option.Id == id);
            if (optionToRemove != null)
            {
                _selectOptions.Remove(optionToRemove);
            }
        }

        public bool Exists(Guid id)
        {
            return _selectOptions.Any(option => option.Id == id);
        }
    }
}
