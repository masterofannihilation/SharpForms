using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface ICompletedFormRepository : IApiRepository<CompletedFormEntity>
    {
        IList<CompletedFormEntity> GetAllFiltered(Guid? formId, Guid? userId);
    }
}
