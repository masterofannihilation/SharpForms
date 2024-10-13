using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.DAL.Memory.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly IList<AnswerEntity> _answers;
        private readonly IList<QuestionEntity> _questions;
        private readonly IList<CompletedFormEntity> _completedForms;
        private readonly IList<SelectOptionEntity> _selectOptions;

        public AnswerRepository(Storage storage)
        {
            _answers = storage.Answers;
            _questions = storage.Questions;
            _completedForms = storage.CompletedForms;
            _selectOptions = storage.SelectOptions;
        }

        public IList<AnswerEntity> GetAll()
        {
            return _answers.ToList(); // Return a new list to avoid modifications
        }

        public AnswerEntity? GetById(Guid id)
        {
            var answer = _answers.SingleOrDefault(answer => answer.Id == id);
            if (answer == null)
            {
                return null;
            }

            answer.Question = _questions.SingleOrDefault(q => q.Id == answer.QuestionId);
            answer.CompletedForm = _completedForms.SingleOrDefault(cf => cf.Id == answer.CompletedFormId);
            
            // If SelectOptionId is not null, load SelectOption
            if (answer.SelectOptionId != null)
            {
                answer.SelectOption = _selectOptions.SingleOrDefault(so => so.Id == answer.SelectOptionId);
            }

            return answer;
        }

        public Guid Insert(AnswerEntity answer)
        {
            _answers.Add(answer);
            return answer.Id; // Return the ID of the newly added answer
        }

        public Guid? Update(AnswerEntity answer)
        {
            var existingAnswer = _answers.SingleOrDefault(a => a.Id == answer.Id);
            if (existingAnswer == null)
            {
                return null;
            }

            existingAnswer.QuestionId = answer.QuestionId;
            existingAnswer.CompletedFormId = answer.CompletedFormId;
            existingAnswer.TextAnswer = answer.TextAnswer;
            existingAnswer.NumberAnswer = answer.NumberAnswer;
            existingAnswer.SelectOptionId = answer.SelectOptionId;

            return existingAnswer.Id;
        }

        public void Remove(Guid id)
        {
            var answerToRemove = _answers.SingleOrDefault(a => a.Id == id);
            if (answerToRemove != null)
            {
                _answers.Remove(answerToRemove);
            }
        }

        public bool Exists(Guid id)
        {
            return _answers.Any(answer => answer.Id == id);
        }
    }
}
