using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Security;
using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.DAL.Memory.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly IList<FormEntity> _forms;
        private readonly IList<QuestionEntity> _questions;
        private readonly IList<CompletedFormEntity> _completedForms;
        private readonly IList<AnswerEntity> _answers;
        private readonly IList<SelectOptionEntity> _selectOptions;
        private readonly IList<UserEntity> _users;
        private readonly IMapper _mapper;

        public FormRepository(Storage storage, IMapper mapper)
        {
            _forms = storage.Forms;
            _questions = storage.Questions;
            _completedForms = storage.CompletedForms;
            _answers =storage.Answers;
            _selectOptions = storage.SelectOptions;
            _users = storage.Users;
            _mapper = mapper;
        }

        public IList<FormEntity> GetAll()
        {
            return _forms.ToList();
        }

        public FormEntity? GetById(Guid id)
        {
            var formEntity = _forms.SingleOrDefault(form => form.Id == id);

            if (formEntity == null)
            {
                return null;
            }

            formEntity.Questions = formEntity.Questions ?? new List<QuestionEntity>();
            formEntity.Creator = _users.SingleOrDefault(user => user.Id == formEntity.CreatorId);
            formEntity.Completions = _completedForms.Where(cf => cf.FormId == formEntity.Id).ToList();

            return formEntity;
        }

        public Guid Insert(FormEntity form)
        {
            _forms.Add(form);
            return form.Id;
        }

        public Guid? Update(FormEntity form)
        {
            var existingForm = _forms.SingleOrDefault(f => f.Id == form.Id);

            if (existingForm == null)
            {
                return null; // Null if not found
            }

            _mapper.Map(form, existingForm); // Update properties to existing entity
            return existingForm.Id;
        }

        public void Remove(Guid id)
        {
            var formToRemove = _forms.SingleOrDefault(f => f.Id == id);
            if (formToRemove == null)
            {
                return;
            }

            // Remove related CompletedFormEntity instances
            var relatedCompletions = _completedForms.Where(cf => cf.FormId == id).ToList();
            foreach (var completion in relatedCompletions)
            {
                // Remove answers from the completed form
                var answerIdsToRemove = completion.Answers.Select(a => a.Id).ToList();
                foreach (var answerId in answerIdsToRemove)
                {
                    var answerToRemove = _answers.SingleOrDefault(a => a.Id == answerId);
                    if (answerToRemove != null)
                    {
                        _answers.Remove(answerToRemove);
                    }
                }
                _completedForms.Remove(completion); // Remove the completed form itself
            }

            // Remove related questions to the form
            var relatedQuestions = _questions.Where(q => q.FormId == formToRemove.Id).ToList();
            foreach (var question in relatedQuestions)
            {
                // Remove answers for each question
                foreach (var answer in question.Answers.ToList())
                {
                    _answers.Remove(answer);
                }
                // Step 6: Remove related select options if applicable
                var relatedSelectOptions = _selectOptions.Where(so => so.QuestionId == question.Id).ToList();
                foreach (var selectOption in relatedSelectOptions)
                {
                    _selectOptions.Remove(selectOption); // Remove select options
                }
                
                _questions.Remove(question); // Remove the question itself
            }

            // Remove the form itself
            _forms.Remove(formToRemove);
        }

        public bool Exists(Guid id)
        {
            return _forms.Any(form => form.Id == id);
        }
    }
}
