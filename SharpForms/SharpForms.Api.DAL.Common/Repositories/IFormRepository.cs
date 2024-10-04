using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface IFormRepository : IApiRepository<FormEntity>
    {
        IList<FormEntity> GetAll();
        FormEntity? GetById(Guid id);
        Guid Insert(FormEntity form);
        Guid? Update(FormEntity form);
        void Remove(Guid id);
        bool Exists(Guid id);
    }
}
