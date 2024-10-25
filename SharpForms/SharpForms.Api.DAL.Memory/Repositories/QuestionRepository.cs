using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.DAL.Memory.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IList<QuestionEntity> _questions;
        private readonly IList<FormEntity> _forms;
        private readonly IList<SelectOptionEntity> _options;
        private readonly IList<AnswerEntity> _answers;
        private readonly IMapper _mapper;

        public QuestionRepository(Storage storage, IMapper mapper)
        {
            _questions = storage.Questions;
            _forms = storage.Forms;
            _options = storage.SelectOptions;
            _answers = storage.Answers;
            _mapper = mapper;
        }

        public IList<QuestionEntity> GetAll()
        {
            return _questions.Select(question => question.DeepCopy()).ToList();
        }

        public QuestionEntity? GetById(Guid id)
        {
            var questionEntity = _questions.SingleOrDefault(question => question.Id == id);

            if (questionEntity == null) return null;

            // Load related data and return a deep copy
            var clonedQuestion = questionEntity.DeepCopy();
            clonedQuestion.Form = _forms.SingleOrDefault(form => form.Id == clonedQuestion.FormId);
            clonedQuestion.Options = _options.Where(so => so.QuestionId == id).Select(option => option.DeepCopy()).ToList();
            clonedQuestion.Answers = _answers.Where(a => a.QuestionId == id).Select(answer => answer.DeepCopy()).ToList();

            return clonedQuestion;
        }

        public Guid Insert(QuestionEntity question)
        {
            var questionCopy = question.DeepCopy();
            _questions.Add(questionCopy);
            return questionCopy.Id;
        }

        public Guid? Update(QuestionEntity question)
        {
            var existingQuestion = _questions.SingleOrDefault(q => q.Id == question.Id);

            if (existingQuestion == null)
            {
                return null; // Return null if the question is not found
            }

            // Manually update each property of the existingQuestion
            existingQuestion.FormId = question.FormId;
            existingQuestion.Form = question.Form;
            existingQuestion.Order = question.Order;
            existingQuestion.Text = question.Text;
            existingQuestion.Description = question.Description;
            existingQuestion.AnswerType = question.AnswerType;
            existingQuestion.MinNumber = question.MinNumber;
            existingQuestion.MaxNumber = question.MaxNumber;

            // Manually update the Options collection
            existingQuestion.Options.Clear();
            foreach (var option in question.Options)
            {
                existingQuestion.Options.Add(option); // Or deep copy if needed
            }

            // Manually update the Answers collection
            existingQuestion.Answers.Clear();
            foreach (var answer in question.Answers)
            {
                existingQuestion.Answers.Add(answer); // Or deep copy if needed
            }

            return existingQuestion.Id;
        }


        public void Remove(Guid id)
        {
            var questionToRemove = _questions.SingleOrDefault(q => q.Id == id);
            if (questionToRemove == null)
            {
                return;
            }

            //remove all answers associated with this question
            var answersToRemove = _answers.Where(a => a.QuestionId == id).ToList();
            foreach (var answer in answersToRemove)
            {
                _answers.Remove(answer);
            }

            //remove all select options associated with this question
            var optionsToRemove = _options.Where(o => o.QuestionId == id).ToList();
            foreach (var option in optionsToRemove)
            {
                _options.Remove(option);
            }

            //remove the question itself
            _questions.Remove(questionToRemove);
        }

        public bool Exists(Guid id)
        {
            return _questions.Any(question => question.Id == id);
        }

        public IList<QuestionEntity> GetAllFiltered(Guid? formId, string? filterText, string? filterDescription)
        {
            var filtered = _questions.AsQueryable();
            if (formId != null)
            {
                filtered = filtered.Where(a => a.FormId == formId);
            }

            if (filterText != null)
            {
                filtered = filtered.Where(a => a.Text.ToLower().Contains(filterText.ToLower()));
            }

            if (filterDescription != null)
            {
                filtered = filtered.Where(a => a.Description != null);
                filtered = filtered.Where(a => a.Description!.Contains(filterDescription));
            }

            return filtered.Select(question => question.DeepCopy()).ToList();
        }
    }
}
