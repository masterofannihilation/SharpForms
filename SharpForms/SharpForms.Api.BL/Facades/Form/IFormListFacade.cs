using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Form;

namespace SharpForms.Api.BL.Facades.Form;

public interface IFormListFacade : IListModelFacade<FormEntity, FormListModel>
{
    List<FormListModel> SearchAllByName(string name);
    List<FormListModel> GetAllCreatedBy(Guid creatorId);
}
