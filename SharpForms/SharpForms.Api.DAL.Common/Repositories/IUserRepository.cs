using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface IUserRepository : IApiRepository<UserEntity>
    {
        IList<UserEntity> GetAll();
        UserEntity? GetById(Guid id);
        Guid Insert(UserEntity user);
        Guid? Update(UserEntity user);
        void Remove(Guid id);
        bool Exists(Guid id);
    }
}
