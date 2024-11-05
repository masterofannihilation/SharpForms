using AutoMapper;
using SharpForms.Common.Models;
using SharpForms.Api.DAL.Common.Entities.Interfaces;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.BL.Facades.Common;

public abstract class DetailModelFacadeBase<TEntity, TDetailModel>(
    IApiRepository<TEntity> entityRepository,
    IMapper mapper)
    : IDetailModelFacade<TEntity, TDetailModel>
    where TEntity : IEntity
    where TDetailModel : IModel
{
    protected virtual TEntity? FetchEntity(Guid id)
    {
        return entityRepository.GetById(id);
    }
    
    public virtual TDetailModel? GetById(Guid id)
    {
        var entity = FetchEntity(id);
        if (entity == null) return default;
        return mapper.Map<TDetailModel>(entity);
    }

    public virtual Guid? CreateOrUpdate(TDetailModel model)
    {
        if(entityRepository.Exists(model.Id))
            return Update(model)!.Value;
        else
        {
            if (model!.Id == new Guid())
                model.Id = Guid.NewGuid();

            return Create(model);
        }
    }

    public virtual Guid Create(TDetailModel model)
    {
        if (model.Id == new Guid())
            model.Id = Guid.NewGuid();

        var entity = mapper.Map<TEntity>(model);
        return entityRepository.Insert(entity);
    }

    public virtual Guid? Update(TDetailModel model)
    {
        var entity = mapper.Map<TEntity>(model);
        return entityRepository.Update(entity);
    }

    public virtual void Delete(Guid id)
    {
        entityRepository.Remove(id);
    }
}
