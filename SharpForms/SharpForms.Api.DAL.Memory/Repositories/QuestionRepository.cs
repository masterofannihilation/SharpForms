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
            return _questions.ToList();
        }

        public QuestionEntity? GetById(Guid id)
        {
            var questionEntity = _questions.SingleOrDefault(question => question.Id == id);

            if (questionEntity == null)
            {
                return null;
            }

            // Load related data
            questionEntity.Form = _forms.SingleOrDefault(form => form.Id == questionEntity.FormId);
            questionEntity.Options = _options.Where(so => so.QuestionId == id).ToList();
            questionEntity.Answers = _answers.Where(a => a.QuestionId == id).ToList();

            return questionEntity;
        }

        public Guid Insert(QuestionEntity question)
        {
            _questions.Add(question);
            return question.Id;
        }

        public Guid? Update(QuestionEntity question)
        {
            var existingQuestion = _questions.SingleOrDefault(q => q.Id == question.Id);

            if (existingQuestion == null)
            {
                return null; // Null if not found
            }

            _mapper.Map(question, existingQuestion); // Updated properties of existing entity
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
    }
}
