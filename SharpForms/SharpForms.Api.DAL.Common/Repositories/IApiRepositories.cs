using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface IApiRepository<TEntity>
        where TEntity : IEntity
    {
        
    }
}
