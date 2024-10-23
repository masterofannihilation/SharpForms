using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Models.Form;

namespace SharpForms.Api.BL.Facades.Form
{
    public class FormDetailFacade : DetailModelFacadeBase<FormEntity, FormDetailModel>, IFormDetailFacade
    {
        private readonly IFormRepository _formRepository;
        private readonly IMapper _mapper;

        public FormDetailFacade(IFormRepository formRepository, IMapper mapper)
            : base(formRepository, mapper)
        {
            _formRepository = formRepository;
            _mapper = mapper;
        }

        public override FormDetailModel? GetById(Guid id)
        {
            var formEntity = _formRepository.GetById(id);
            if (formEntity == null) return null;

            // Map the form entity and include related entities like creator, questions, and completions.
            var model = _mapper.Map<FormDetailModel>(formEntity);
            return model;
        }

        public override Guid? Update(FormDetailModel model)
        {
            var existingEntity = _formRepository.GetById(model.Id);
            if (existingEntity == null) return null;

            existingEntity.Name = model.Name;
            existingEntity.OpenSince = model.OpenSince;
            existingEntity.OpenUntil = model.OpenUntil;
            
            if (model.Creator != null)
            {
                existingEntity.CreatorId = model.Creator.Id;
            }

            return _formRepository.Update(existingEntity);
        }
    }
}
