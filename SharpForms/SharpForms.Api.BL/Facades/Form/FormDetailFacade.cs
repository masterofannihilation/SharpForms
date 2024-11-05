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
    }
}
