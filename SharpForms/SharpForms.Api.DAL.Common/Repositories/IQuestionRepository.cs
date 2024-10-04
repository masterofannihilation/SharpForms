using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface IQuestionRepository
    {
        IList<QuestionEntity> GetAll();
        QuestionEntity? GetById(Guid id);
        Guid Insert(QuestionEntity question);
        Guid? Update(QuestionEntity question);
        void Remove(Guid id);
        bool Exists(Guid id);
    }
}
