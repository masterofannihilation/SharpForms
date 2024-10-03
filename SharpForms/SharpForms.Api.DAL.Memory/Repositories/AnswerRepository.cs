using System;
using System.Collections.Generic;
using System.Linq;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.DAL.Memory.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly IList<AnswerEntity> _answers;

        public AnswerRepository(IList<AnswerEntity> answersStorage)
        {
            this._answers = answersStorage;
        }

        public IList<AnswerEntity> GetAll()
        {
            return _answers.ToList(); // Return a new list to avoid modifications
        }

        public AnswerEntity? GetById(Guid id)
        {
            return _answers.SingleOrDefault(answer => answer.Id == id);
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
