using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.CompletedForm;

namespace SharpForms.Api.BL.Facades.CompletedForm;

public class CompletedFormListFacade(ICompletedFormRepository compFormRepository, IMapper mapper)
    : ListModelFacadeBase<CompletedFormEntity, CompletedFormListModel>(compFormRepository, mapper),
        ICompletedFormListFacade
{
    protected override CompletedFormListModel GetEntityToModel(CompletedFormEntity entity)
    {
        var model = base.GetEntityToModel(entity);

        if (entity.User != null)
        {
            model.UserName = entity.User.Name;
        }

        return model;
    }

    public List<CompletedFormListModel> GetAllCopletionsOfForm(Guid formId)
    {
        return compFormRepository.GetAllFiltered(formId, null).AsEnumerable().Select(this.GetEntityToModel).ToList();
    }

    public List<CompletedFormListModel> GetAllCompletionsMadeByUser(Guid userId)
    {
        return compFormRepository.GetAllFiltered(null, userId).AsEnumerable().Select(this.GetEntityToModel).ToList();
    }
}
