using System.Data.Common;
using AutoMapper;
using SharpForms.Common.Models;
using SharpForms.Api.DAL.Common.Entities.Interfaces;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.BL.Facades.Common;

public abstract class ListModelFacadeBase<TEntity, TListModel>(
    IApiRepository<TEntity> entityRepository,
    IMapper modelMapper)
    : IListModelFacade<TEntity, TListModel>
    where TEntity : class, IEntity
    where TListModel : class, IModel
{
    protected virtual IEnumerable<TEntity> GetAllFetchMethod()
    {
        return entityRepository.GetAll().AsEnumerable();
    }

    public List<TListModel> GetAll()
    {
        return GetAllFetchMethod().Select(this.GetEntityToModel).ToList()!;
    }

    public TListModel? GetById(Guid id)
    {
        var entity = entityRepository.GetById(id);
        if (entity == null) return null;
        return GetEntityToModel(entity);
    }

    protected virtual TListModel GetEntityToModel(TEntity entity)
    {
        return modelMapper.Map<TListModel>(entity);
    }
}
