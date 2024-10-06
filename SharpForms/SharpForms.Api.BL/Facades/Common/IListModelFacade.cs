using SharpForms.Common.Models;
using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.BL.Facades.Common;

public interface IListModelFacade<TEntity, TListModel>
    where TEntity : IEntity
    where TListModel : IModel
{
    List<TListModel> GetAll();
    TListModel? GetById(Guid id);
}
