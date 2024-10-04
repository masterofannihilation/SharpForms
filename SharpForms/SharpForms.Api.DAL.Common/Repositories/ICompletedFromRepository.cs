using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface ICompletedFormRepository : IApiRepository<CompletedFormEntity>
    {
        IList<CompletedFormEntity> GetAll();
        CompletedFormEntity? GetById(Guid id);
        Guid Insert(CompletedFormEntity completedForm);
        Guid? Update(CompletedFormEntity completedForm);
        void Remove(Guid id);
        bool Exists(Guid id);
    }
}
