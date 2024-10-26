using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.Form;

namespace SharpForms.Api.BL.Facades.Form;

public class FormListFacade(IFormRepository formRepository, IMapper mapper)
    : ListModelFacadeBase<FormEntity, FormListModel>(formRepository, mapper), IFormListFacade
{
    public List<FormListModel> SearchAllByName(string name)
    {
        return formRepository.GetAllFiltered(name, null).AsEnumerable().Select(this.GetEntityToModel).ToList();
    }

    public List<FormListModel> GetAllCreatedBy(Guid creatorId)
    {
        return formRepository.GetAllFiltered(null, creatorId).AsEnumerable().Select(this.GetEntityToModel).ToList();
    }
}
