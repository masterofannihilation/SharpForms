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
    public List<TListModel> GetAll()
    {
        return entityRepository.GetAll().Select(this.GetEntityToModel).Where(m => m != null).ToList()!;
    }

    public TListModel? GetById(Guid id)
    {
        return GetEntityToModel(entityRepository.GetById(id));
    }

    protected virtual TListModel? GetEntityToModel(TEntity? entity)
    {
        return entity == null ? null : modelMapper.Map<TListModel>(entity);
    }
}
