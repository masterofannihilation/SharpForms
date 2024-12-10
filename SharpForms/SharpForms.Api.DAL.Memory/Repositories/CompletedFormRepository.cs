using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.DAL.Memory.Repositories
{
    public class CompletedFormRepository : ICompletedFormRepository
    {
        private readonly IList<CompletedFormEntity> _completedForms;
        private readonly IList<AnswerEntity> _answers;
        private readonly IList<UserEntity> _users;
        private readonly IList<FormEntity> _forms;
        private readonly IMapper _mapper;

        public CompletedFormRepository(Storage storage, IMapper mapper)
        {
            _completedForms = storage.CompletedForms;
            _answers = storage.Answers;
            _users = storage.Users;
            _forms = storage.Forms;
            _mapper = mapper;
        }

        public IList<CompletedFormEntity> GetAll()
        {
            var cfs = new List<CompletedFormEntity>();
            foreach (var f in _completedForms)
            {
                cfs.Add(IncludeEntities(f.CreateSafeCopy()));
            }
            
            return cfs;
        }

        private CompletedFormEntity IncludeEntities(CompletedFormEntity completedForm)
        {
            completedForm.Answers = GetAnswersByCompletedFormId(completedForm.Id);
            completedForm.Form = _forms.SingleOrDefault(form => form.Id == completedForm.FormId);
            completedForm.User = _users.SingleOrDefault(user => user.Id == completedForm.UserId);

            return completedForm;
        }

        public CompletedFormEntity? GetById(Guid id)
        {
            var completedForm = _completedForms.SingleOrDefault(form => form.Id == id);
            if (completedForm == null)
                return null;

            return IncludeEntities(completedForm.CreateSafeCopy());
        }

        public Guid Insert(CompletedFormEntity completedForm)
        {
            var completedFormCopy = completedForm.CreateSafeCopy();
            _completedForms.Add(completedFormCopy);
            return completedFormCopy.Id;
        }

        public Guid? Update(CompletedFormEntity completedForm)
        {
            var existingForm = _completedForms.SingleOrDefault(form => form.Id == completedForm.Id);
            if (existingForm == null) return null;

            _mapper.Map(completedForm, existingForm);
            
            return existingForm.Id;
        }

        public void Remove(Guid id)
        {
            var formToRemove = _completedForms.SingleOrDefault(form => form.Id == id);
            if (formToRemove == null)
                return;

            // Remove all answers associated with this completed form
            var answersToRemove = _answers.Where(answer => answer.CompletedFormId == id).ToList();
            foreach (var answer in answersToRemove)
                _answers.Remove(answer);

            _completedForms.Remove(formToRemove);
        }

        public bool Exists(Guid id)
        {
            return _completedForms.Any(form => form.Id == id);
        }

        public IList<CompletedFormEntity> GetAllFiltered(Guid? formId, Guid? userId)
        {
            var filtered = _completedForms.AsQueryable();
            if (formId != null)
                filtered = filtered.Where(cf => cf.FormId == formId);

            if (userId != null)
                filtered = filtered.Where(cf => cf.UserId == userId);

            var cfs = new List<CompletedFormEntity>();
            foreach (var cf in filtered)
                cfs.Add(IncludeEntities(cf.CreateSafeCopy()));

            return cfs;
        }

        private IList<AnswerEntity> GetAnswersByCompletedFormId(Guid completedFormId)
        {
            return _answers.Where(answer => answer.CompletedFormId == completedFormId).ToList();
        }
    }
}
