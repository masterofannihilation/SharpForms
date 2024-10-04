using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface ISelectOptionRepository
    {
        IList<SelectOptionEntity> GetAll();
        SelectOptionEntity? GetById(Guid id);
        Guid Insert(SelectOptionEntity selectOption);
        Guid? Update(SelectOptionEntity selectOption);
        void Remove(Guid id);
        bool Exists(Guid id);
    }
}
