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
        private readonly IList<UserEntity> _users;

        public AnswerRepository(Storage storage)
        {
            _answers = storage.Answers;
            _questions = storage.Questions;
            _completedForms = storage.CompletedForms;
            _selectOptions = storage.SelectOptions;
            _users = storage.Users;
        }

        public IList<AnswerEntity> GetAll()
        {
            return _answers.Select(answer => answer.DeepCopy()).ToList(); 
        }

        private AnswerEntity IncludeEntities(AnswerEntity answer)
        {
            var answerCopy = answer.DeepCopy();

            answerCopy.Question = _questions.SingleOrDefault(q => q.Id == answerCopy.QuestionId);
            answerCopy.CompletedForm = _completedForms.SingleOrDefault(cf => cf.Id == answerCopy.CompletedFormId);

            // If SelectOptionId is not null, load SelectOption
            if (answerCopy.SelectOptionId != null)
            {
                answerCopy.SelectOption = _selectOptions.SingleOrDefault(so => so.Id == answerCopy.SelectOptionId);
            }

            if (answerCopy.CompletedForm != null)
            {
                answerCopy.CompletedForm.User = _users.SingleOrDefault(u => u.Id == answerCopy.CompletedForm.UserId);
            }

            return answerCopy;
        }

        public AnswerEntity? GetById(Guid id)
        {
            var answer = _answers.SingleOrDefault(answer => answer.Id == id);
            if (answer == null)
            {
                return null;
            }

            return IncludeEntities(answer);
        }

        public Guid Insert(AnswerEntity answer)
        {
            // insert a deep copy of the original as the original might be modified
            var answerCopy = answer.DeepCopy();
            _answers.Add(answerCopy);
            return answerCopy.Id;
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

        public IList<AnswerEntity> GetAllWithUser(Guid? completedFormId, Guid? questionId)
        {
            var filtered = _answers.AsQueryable();
            if (completedFormId != null)
            {
                filtered = filtered.Where(a => a.CompletedFormId == completedFormId);
            }

            if (questionId != null)
            {
                filtered = filtered.Where(a => a.QuestionId == questionId);
            }

            var answers = new List<AnswerEntity>();
            foreach (var answer in filtered)
            {
                answers.Add(IncludeEntities(answer));
            }

            return answers;
        }
    }
}
