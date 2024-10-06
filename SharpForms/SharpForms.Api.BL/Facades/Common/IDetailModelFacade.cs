using SharpForms.Common.Models;
using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.BL.Facades.Common;

public interface IDetailModelFacade<TEntity, TDetailModel>
    where TEntity : IEntity
    where TDetailModel : IModel
{
    TDetailModel? GetById(Guid id);
    Guid CreateOrUpdate(TDetailModel model);
    Guid Create(TDetailModel model);
    Guid? Update(TDetailModel model);
    void Delete(Guid id);
}
