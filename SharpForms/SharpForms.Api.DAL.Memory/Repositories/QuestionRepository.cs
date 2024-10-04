using System;
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
        private readonly IMapper _mapper;

        public QuestionRepository(Storage storage, IMapper mapper)
        {
            _questions = storage.Questions;
            _forms = storage.Forms;
            _options = storage.SelectOptions;
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
                return questionEntity;
            }

            // Load related data
            questionEntity.Form = _forms.SingleOrDefault(form => form.Id == questionEntity.FormId);
            questionEntity.Options = questionEntity.Options ?? new List<SelectOptionEntity>();
            questionEntity.Answers = questionEntity.Answers ?? new List<AnswerEntity>();

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
            if (questionToRemove != null)
            {
                _questions.Remove(questionToRemove);
            }
        }

        public bool Exists(Guid id)
        {
            return _questions.Any(question => question.Id == id);
        }
    }
}
