using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Memory.Repositories;

public interface IAnswerRepository
{
    IList<AnswerEntity> GetAll();
    AnswerEntity? GetById(Guid id);
    Guid Insert(AnswerEntity answer);
    Guid? Update(AnswerEntity answer);
    void Remove(Guid id);
    bool Exists(Guid id);
}
