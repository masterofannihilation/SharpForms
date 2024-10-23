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
        private readonly IList<CompletedFormEntity> completedForms;
        private readonly IList<AnswerEntity> answers;
        private readonly IList<UserEntity> users;
        private readonly IList<FormEntity> forms;
        private readonly IMapper mapper;

        public CompletedFormRepository(
            Storage storage,
            IMapper mapper)
        {
            this.completedForms = storage.CompletedForms;
            this.answers = storage.Answers;
            this.users = storage.Users;
            this.forms = storage.Forms;
            this.mapper = mapper;
        }

        public IList<CompletedFormEntity> GetAll()
        {
            var cfs = new List<CompletedFormEntity>();
            foreach (var f in completedForms)
            {
                cfs.Add(IncludeEntities(f));
            }

            return cfs;
        }

        private CompletedFormEntity IncludeEntities(CompletedFormEntity completedForm)
        {
            completedForm.Answers = GetAnswersByCompletedFormId(completedForm.Id);
            completedForm.Form = forms.SingleOrDefault(form => form.Id == completedForm.FormId);
            completedForm.User = users.SingleOrDefault(user => user.Id == completedForm.UserId);

            return completedForm;
        }

        public CompletedFormEntity? GetById(Guid id)
        {
            var completedForm = completedForms.SingleOrDefault(form => form.Id == id);
            if (completedForm == null)
            {
                return null;
            }

            return IncludeEntities(completedForm);
        }

        public Guid Insert(CompletedFormEntity completedForm)
        {
            var completedFormCopy = completedForm.DeepCopy();
            completedForms.Add(completedFormCopy);
            return completedFormCopy.Id;
        }

        public Guid? Update(CompletedFormEntity completedForm)
        {
            var existingForm = completedForms.SingleOrDefault(form => form.Id == completedForm.Id);
            if (existingForm != null)
            {
                // Use AutoMapper to map the properties from completedForm to existingForm
                mapper.Map(completedForm, existingForm);
                return existingForm.Id;
            }
            return null; // Null if not found
        }

        public void Remove(Guid id)
        {
            var formToRemove = completedForms.SingleOrDefault(form => form.Id == id);
            if (formToRemove == null)
            {
                return;
            }

            // Remove all answers associated with this completed form
            var answersToRemove = answers.Where(answer => answer.CompletedFormId == id).ToList();
            foreach (var answer in answersToRemove)
            {
                answers.Remove(answer);
            }

            completedForms.Remove(formToRemove);
        }

        public bool Exists(Guid id)
        {
            return completedForms.Any(form => form.Id == id);
        }

        public IList<CompletedFormEntity> GetAllFiltered(Guid? formId, Guid? userId)
        {
            var filtered = completedForms.AsQueryable();
            if (formId != null)
            {
                filtered = filtered.Where(cf => cf.FormId == formId);
            }

            if (userId != null)
            {
                filtered = filtered.Where(cf => cf.UserId == userId);
            }

            var cfs = new List<CompletedFormEntity>();
            foreach (var cf in filtered)
            {
                cfs.Add(IncludeEntities(cf));
            }

            return cfs;
        }

        private IList<AnswerEntity> GetAnswersByCompletedFormId(Guid completedFormId)
        {
            return answers.Where(answer => answer.CompletedFormId == completedFormId).ToList();
        }
    }
}
