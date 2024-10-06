using AutoMapper;
using SharpForms.Common.Models;
using SharpForms.Api.DAL.Common.Entities.Interfaces;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.BL.Facades.Common;

public abstract class DetailModelFacadeBase<TEntity, TDetailModel>(
    IApiRepository<TEntity> entityRepository,
    IMapper modelMapper)
    : IDetailModelFacade<TEntity, TDetailModel>
    where TEntity : IEntity
    where TDetailModel : IModel
{
    public virtual TDetailModel? GetById(Guid id)
    {
        var entity = entityRepository.GetById(id);
        return modelMapper.Map<TDetailModel>(entity);
    }

    public Guid CreateOrUpdate(TDetailModel model)
    {
        return entityRepository.Exists(model.Id)
            ? Update(model)!.Value
            : Create(model);
    }

    public virtual Guid Create(TDetailModel model)
    {
        var entity = modelMapper.Map<TEntity>(model);
        return entityRepository.Insert(entity);
    }

    public virtual Guid? Update(TDetailModel model)
    {
        var entity = modelMapper.Map<TEntity>(model);
        return entityRepository.Update(entity);
    }

    public virtual void Delete(Guid id)
    {
        entityRepository.Remove(id);
    }
}
