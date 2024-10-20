using SharpForms.Common.Models;
using SharpForms.Api.DAL.Common.Entities.Interfaces;
using SharpForms.Common.BL.Facades;

namespace SharpForms.Api.BL.Facades.Common;

public interface IListModelFacade<TEntity, TListModel> : IAppFacade
    where TEntity : IEntity
    where TListModel : IModel
{
    List<TListModel> GetAll();
    TListModel? GetById(Guid id);
}
