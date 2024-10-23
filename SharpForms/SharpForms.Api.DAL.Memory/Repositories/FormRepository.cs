using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.DAL.Memory.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly IList<FormEntity> _forms;
        private readonly IMapper _mapper;

        public FormRepository(Storage storage, IMapper mapper)
        {
            _forms = storage.Forms;
            _mapper = mapper;
        }

        public IList<FormEntity> GetAll()
        {
            return _forms.Select(form => form.DeepCopy()).ToList();
        }

        public FormEntity? GetById(Guid id)
        {
            var formEntity = _forms.SingleOrDefault(form => form.Id == id);
            if (formEntity == null) return null;

            formEntity.Questions = formEntity.Questions ?? new List<QuestionEntity>();
            return formEntity.DeepCopy();
        }

        public Guid Insert(FormEntity form)
        {
            var formCopy = form.DeepCopy();
            _forms.Add(formCopy);
            return formCopy.Id;
        }

        public Guid? Update(FormEntity form)
        {
            var existingForm = _forms.SingleOrDefault(f => f.Id == form.Id);

            if (existingForm == null) return null;

            _mapper.Map(form, existingForm); 
            return existingForm.Id;
        }

        public void Remove(Guid id)
        {
            var formToRemove = _forms.SingleOrDefault(f => f.Id == id);
            if (formToRemove != null)
            {
                _forms.Remove(formToRemove);
            }
        }

        public bool Exists(Guid id)
        {
            return _forms.Any(form => form.Id == id);
        }
    }
}
