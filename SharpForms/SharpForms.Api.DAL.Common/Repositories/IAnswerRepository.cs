using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface IAnswerRepository : IApiRepository<AnswerEntity>
    {
        /// <summary>
        /// Returns filtered list of answer entities with UserEntity loaded too.
        /// And also optionally filters answers related to a completion of a form
        /// or question.
        IList<AnswerEntity> GetAllWithUser(Guid? completedFormId, Guid? questionId);
    }
}
