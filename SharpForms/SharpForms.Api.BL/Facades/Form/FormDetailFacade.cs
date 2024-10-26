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
    }
}
