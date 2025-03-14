using System;
using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.DAL.Common.Repositories
{
    public interface IUserRepository : IApiRepository<UserEntity>
    {
        IList<UserEntity> GetAllFiltered(string name);
    }
}
