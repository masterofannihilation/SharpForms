using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface IQuestionRepository : IApiRepository<QuestionEntity>
    {
        IList<QuestionEntity> GetAllFiltered(Guid? formId, string? filterText, string? filterDescription);
    }
}
