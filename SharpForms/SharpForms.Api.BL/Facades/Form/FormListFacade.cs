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
    protected override FormListModel GetEntityToModel(FormEntity entity)
    {
        var model = base.GetEntityToModel(entity);

        if (entity.Creator != null)
        {
            model.CreatorName = entity.Creator.Name;
        }

        model.QuestionCount = entity.Questions.Count();
        model.TimesCompleted = entity.Completions.Count();

        return model;
    }

    public List<FormListModel> SearchAllByName(string name)
    {
        return formRepository.GetAllFiltered(name, null).AsEnumerable().Select(this.GetEntityToModel).ToList();
    }

    public List<FormListModel> GetAllCreatedBy(Guid creatorId)
    {
        return formRepository.GetAllFiltered(null, creatorId).AsEnumerable().Select(this.GetEntityToModel).ToList();
    }
}
