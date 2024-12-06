using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Models.Form;

namespace SharpForms.Api.BL.Facades.Form
{
    public class FormDetailFacade(IFormRepository formRepository, IMapper mapper)
        : DetailModelFacadeBase<FormEntity, FormDetailModel>(formRepository, mapper), IFormDetailFacade
    {
        private readonly IFormRepository _formRepository = formRepository;
        private readonly IMapper _mapper = mapper;
        
        // New validation method for dates
        private void ValidateFormDates(FormDetailModel formDetailModel)
        {
            if (formDetailModel.OpenSince.HasValue && formDetailModel.OpenSince.Value < DateTime.Today)
            {
                throw new InvalidOperationException("The 'OpenSince' date must be today or in the future.");
            }

            if (formDetailModel.OpenUntil.HasValue && formDetailModel.OpenUntil.Value <= formDetailModel.OpenSince)
            {
                throw new InvalidOperationException("The 'OpenUntil' date must be later than the 'OpenSince' date.");
            }
        }

        public Task<Guid> CreateAsync(FormDetailModel formDetailModel)
        {
            // Validate dates before proceeding with the creation
            ValidateFormDates(formDetailModel);

            // Ensure the name is provided and valid
            if (string.IsNullOrWhiteSpace(formDetailModel.Name))
            {
                throw new InvalidOperationException("The 'Name' field is required.");
            }

            // Map the FormDetailModel to FormEntity
            var formEntity = _mapper.Map<FormEntity>(formDetailModel);

            // Insert the form into the repository and get the ID
            Guid formId = _formRepository.Insert(formEntity);

            return Task.FromResult(formId);
        }
    }
}
