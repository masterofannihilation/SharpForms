using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface IApiRepository<TEntity>
        where TEntity : IEntity
    {
        IList<TEntity> GetAll();
        TEntity? GetById(Guid id);
        Guid Insert(TEntity entity);
        Guid? Update(TEntity entity);
        void Remove(Guid id);
        bool Exists(Guid id);
    }
}
